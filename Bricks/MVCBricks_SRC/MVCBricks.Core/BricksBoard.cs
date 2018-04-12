using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MVCBricks.Core.Exceptions;
using MVCBricks.Core.Shapes;
using System.Drawing;

namespace MVCBricks.Core
{
    public class BricksBoard : BaseBricksArray, IBoard
    {
        #region attribute
        protected Color backColor = Color.Black;
        private IShape shape = null;
        private int score = 0;
        private int hiScore = 0;
        private int level = 1;
        private int lines = 0;
        private IShape next = null;
        private bool isPlaying = false;
        #endregion attribute

        #region constructors
        public BricksBoard(IPresenter presenter)
        {
            this.presenter = presenter;
            this.width = 10;
            this.height = 16;
            InitializeArray();
            next = GetRandomShape();
        }

        public BricksBoard(IPresenter presenter, int width, int height)
        {
            this.presenter = presenter;

            if (width < 0)
                throw new ArgumentOutOfRangeException("width");

            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            this.width = width;
            this.height = height;
            InitializeArray();
            next = GetRandomShape();
        }

        #endregion constructors

        #region methods

        public override void InitializeArray()
        {
            score = 0;
            level = 1;
            lines = 0;
            if (shape != null)
            {
                shape.Y = 0;
            }
            next = GetRandomShape();
            presenter.UpdateScoreView(score, hiScore, lines, level, next);
            base.InitializeArray();
        }

        public bool TestPieceOnPosition(IShape shape, int x, int y)
        {
            for (int row = 0; row < shape.Height; row++)
            {
                for (int column = 0; column < shape.Width; column++)
                {
                    //is the position out of range?
                    if (column + x < 0)
                        return false;

                    if (row + y < 0)
                        return false;

                    if (column + x >= width)
                        return false;

                    if (row + y >= height)
                        return false;

                    //will the shape collide in the board?
                    if (
                        shapeArray[column + x, row + y] != null &&
                        shape.ShapeArray[column, row] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void RemovePieceFromCurrentPosition(IShape shape)
        {
            for (int row = 0; row < shape.Height; row++)
            {
                for (int column = 0; column < shape.Width; column++)
                {
                    if (shape.ShapeArray[column, row] != null)
                    {
                        shapeArray[column + shape.X, row + shape.Y] = null;
                    }
                }
            }
        }

        public void PutPieceOnPosition(IShape shape, int x, int y)
        {
            if (!TestPieceOnPosition(shape, x, y))
                throw new CantSetShapePosition();

            for (int row = 0; row < shape.Height; row++)
            {
                for (int column = 0; column < shape.Width; column++)
                {
                    if (shape.ShapeArray[column, row] != null)
                    {
                        shapeArray[column + x, row + y] = shape.ShapeArray[column, row];
                    }
                }
            }
            shape.X = x;
            shape.Y = y;

            if (presenter != null)
            {
                presenter.UpdateBoardView(GetStringFromShapeArray(), shapeArray, width, height);
            }
        }

        private bool RemoveCompletedRows()
        {
            bool completed = false;
            int row = height - 1;
            while (row >= 0)
            {
                completed = true;
                for (int column = 0; column < width; column++)
                {
                    if (shapeArray[column, row] == null)
                    {
                        completed = false;
                        break;
                    }
                }

                if (completed)
                {
                    //presenter.HighlightCompletedRow(row);

                    IBrick[] removedBricks = new IBrick[width];
                    for (int column = 0; column < width; column++)
                    {
                        removedBricks[column] = shapeArray[column, row];
                    }

                    shape = null;
                    for (int innerRow = row; innerRow > 0; innerRow--)
                    {
                        for (int innerColumn = 0; innerColumn < width; innerColumn++)
                        {
                            shapeArray[innerColumn, innerRow] = shapeArray[innerColumn, innerRow - 1];
                            shapeArray[innerColumn, innerRow - 1] = null;
                        }
                    }

                    score += 10 * level;
                    if (score > hiScore)
                    {
                        hiScore = score;
                    }
                    lines++;
                    level = 1 + (lines / 10);
                    presenter.UpdateScoreView(score, hiScore, lines, level, next);
                }
                else
                {
                    row--;
                }
            }

            if (presenter != null)
            {
                presenter.UpdateBoardView(GetStringFromShapeArray(), shapeArray, width, height);
            }

            if (completed)
            {
                RemoveCompletedRows();
            }
            return completed;
        }

        public void ProcessNextMove()
        {
            if (shape == null)
            {
                StartRandomShape();
            }

            bool couldMoveDown = true;

            if (!shape.Anchored)
            {
                RemovePieceFromCurrentPosition(shape);
                couldMoveDown = shape.MoveDown();
            }
            else
            {
                bool full = !StartRandomShape();
                if (full)
                {
                    InitializeArray();
                    GameOver();
                    return;
                }
                else
                {
                    couldMoveDown = shape.MoveDown();
                }
            }

            if (!couldMoveDown)
            {
                RemoveCompletedRows();
            }

            if (presenter != null)
            {
                presenter.UpdateBoardView(GetStringFromShapeArray(), shapeArray, width, height);
            }
        }

        private void GameOver()
        {
            level = 1;
            lines = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("");
            presenter.UpdateBoardView(this.shapeString, shapeArray, width, height);
            presenter.GameOver();
        }

        public bool StartRandomShape()
        {
            if (shape != null && !shape.Anchored)
            {
                this.RemovePieceFromCurrentPosition(shape);
            }

            shape = next;
            
            next = GetRandomShape();
            shape.ContainerBoard = this;
            int x = (this.Width - shape.Width) / 2;

            bool ret = this.TestPieceOnPosition(shape, x, 0);
            if (ret)
            {
                try
                {
                    this.PutPieceOnPosition(shape, x, 0);
                }
                catch {}
            }
            return ret;
        }

        private IShape GetRandomShape()
        {
            IShape newShape = null;
            Random randomClass = new Random();
            int randomCode = randomClass.Next((int)ShapeCodes.I, (int)ShapeCodes.Z + 1);

            switch (randomCode)
            {
                case (int)ShapeCodes.I:
                    newShape = new StickShape();
                    newShape.Color = Color.Cyan;
                    break;
                case (int)ShapeCodes.J:
                    newShape = new JShape();
                    newShape.Color = Color.Blue;
                    break;
                case (int)ShapeCodes.L:
                    newShape = new LShape();
                    newShape.Color = Color.Orange;
                    break;
                case (int)ShapeCodes.O:
                    newShape = new OShape();
                    newShape.Color = Color.Yellow;
                    break;
                case (int)ShapeCodes.S:
                    newShape = new SShape();
                    newShape.Color = Color.Green;
                    break;
                case (int)ShapeCodes.T:
                    newShape = new TShape();
                    newShape.Color = Color.Purple;
                    break;
                case (int)ShapeCodes.Z:
                    newShape = new ZShape();
                    newShape.Color = Color.Red;
                    break;
            }

            ((BaseShape)newShape).Presenter = presenter;

            presenter.UpdateScoreView(score, hiScore, lines, level, newShape);
            return newShape;
        }

        public bool MoveLeft()
        {
            if (shape == null)
            {
                return false;
            }
            else
            {
                return shape.MoveLeft();
            }
        }

        public bool MoveRight()
        {
            if (shape == null)
            {
                return false;
            }
            else
            {
                return shape.MoveRight();
            }
        }

        public bool MoveDown()
        {
            if (shape == null)
            {
                return false;
            }
            else
            {
                bool ret = shape.MoveDown();
                if (shape.Anchored)
                {
                    RemoveCompletedRows();
                }
                return ret;
            }
        }

        public bool Rotate90()
        {
            if (shape == null)
            {
                return false;
            }
            else
            {
                return shape.Rotate90();
            }
        }

        public bool Rotate270()
        {
            if (shape == null)
            {
                return false;
            }
            else
            {
                return shape.Rotate270();
            }
        }

        #endregion methods

        #region properties
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public int Score
        {
            get { return score; }
        }

        private int HiScore
        {
            get { return hiScore; }
        }

        public int Level
        {
            get { return level; }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
        }
        #endregion properties
    }
}
