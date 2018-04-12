﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <script type="text/javascript" src="../../Scripts/fsm.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-1.4.4.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery.timers.js"></script>
    <title>BricksView</title>
    <style type="text/css">
        html
        {
            background-color: Black;
        }
        span
        {
            padding: 0px;
            margin: 0px;
        }
        .screen
        {
            margin-left: 10%;
            margin-right: 10%;
            position: relative;
        }

        .centerPanel
        {
            width: 500px;
            left: 50%;
            margin-top: -100px;
            margin-left: -250px;
            position: relative;
        }

        .board
        {
            width: 262px;
            left: 50%;
            margin-left: -131px;
            position: relative;
            background-image: -webkit-gradient(linear, left top, right bottom, color-stop(0.0, #222), color-stop(0.5, #111), color-stop(1.0, #222));
            float: left;
            opacity: 0.0;
            visibility: hidden;
        }
        
        .scorePanel
        {
            font-family: Showcard Gothic;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            color: Gray;
            position: relative;
            display: block;
            float: right;
            opacity: 0.0;
            visibility: hidden;
        }
        
        root
        {
            display: block;
        }
        
        .colorChip
        {
            position: relative;
            float: left;
            width: 20px;
            height: 20px;
            margin: 2px;
            border-style: solid;
            border-width: 1px;
            border-color: #333333;
        }
        
        .clear
        {
            clear: both;
        }
        
        .clearfix
        {
            display: inline-block;
        }
        
        .clearfix:after, .container:after
        {
            clear: both;
            content: ".";
            display: block;
            height: 0;
            visibility: hidden;
        }
        
        * html .clearfix
        {
            height: 0%;
        }
        .clearfix
        {
            display: block;
        }
        
        .title
        {
            top: 428px;
            font-family: Arial Narrow;
            font-size: 40px;
            font-weight: bold;
            text-align: center;
            color: Gray;
            width: 300px;
            left: 50%;
            margin-left: -130px;
            position: relative;
            display: block;
        }

        #gamePaused
        {
            top: 300px;
            visibility: hidden;            
            text-decoration: blink;
            font-family: Lucida Sans;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            color: Orange;
            width: 500px;
            left: 40%;
            margin-left: -100px;
            position: fixed;
            display: block;           
        }
        
        #gameOver
        {
            top: 300px;
            visibility: hidden;            
            text-decoration: blink;
            font-family: Lucida Sans;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            color: Orange;
            width: 500px;
            left: 40%;
            margin-left: -100px;
            position: fixed;
            display: block;           
        }

        .subTitle
        {
            font-family: Lucida Sans;
            font-size: 14px;
            font-weight: bold;
            text-align: center;
            color: Gray;
            width: 400px;
            left: 65%;
            margin-left: -200px;
            position: relative;
            display: block;
        }
        
        .image
        {
            overflow: hidden;
            margin: 0px 0px 0px 0px;
        }
        
        .bob
        {
            vertical-align: middle;
        }
        
        .press
        {
            text-decoration: blink;
            font-family: Lucida Sans;
            font-size: 20px;
            font-weight: bold;
            text-align: center;
            color: Orange;
            width: 400px;
            left: 40%;
            margin-left: -130px;
            position: relative;
            display: block;           
        }        
    </style>
    <script type="text/javascript">

        //Finite State Machine for JavaScript
        //by Anthony Blackshaw
        //http: //antsdev.wordpress.com/2008/06/18/a-simple-js-finite-state-machine/

        var gameState = new FSM("intro");

        gameState.add_transition("play", "intro", changeIntroToPlaying, "playing");        
        gameState.add_transition("pause", "playing", changePlayingToPaused, "paused");
        gameState.add_transition("continue", "paused", changePausedToPlaying, "playing");
        gameState.add_transition("end", "playing", changePlayingToGameOver, "gameOver");
        gameState.add_transition("showIntro", "gameOver", changeGameOverToIntro, "intro");

        function changeIntroToPlaying() {
            initializeBoard();
            $('.subTitle').css('visibility', 'hidden');
            $('.press').css('visibility', 'hidden');

            $('.title').animate({
                top: 0
            }, 1000, 'swing', function () {
                // Animation complete.
                $('.scorePanel').animate({
                    opacity: 1.0
                }, 1000, 'swing', function () {
                    $('.scorePanel').css('visibility', 'visible');
                });

                $('.board').animate({
                    opacity: 1.0
                }, 1000, 'swing', function () {
                    $('.board').css('visibility', 'visible');
                });
            });
        }

        function changePlayingToPaused () {
            $('#gamePaused').css('visibility', 'visible');
        }

        function changePausedToPlaying() {
            $('#gamePaused').css('visibility', 'hidden');
        }

        function changePlayingToGameOver () {
            $('#gameOver').css('visibility', 'visible');
        }

        function changeGameOverToIntro() {
            $('#gameOver').css('visibility', 'hidden');

            $('.scorePanel').animate({
                opacity: 0.0
            }, 1000, 'swing', function () {
                $('.scorePanel').css('visibility', 'hidden');
            });

            $('.board').animate({
                opacity: 0.0
            }, 1000, 'swing', function () {
                // Animation complete.
                $('.board').css('visibility', 'hidden');
                $('.title').animate({
                    top: 200
                }, 1000, 'swing', function () {
                    // Animation complete.
                    $('.subTitle').css('visibility', 'visible');
                    $('.press').css('visibility', 'visible');
                });
            });
        }

        $(function () {
            createCells();
            setImages();
            showSplashScreen();
            initializeBoard();
            setTimer();
        });

        function createCells() {
            for (var row = 0; row < 16; row++) {
                for (var col = 0; col < 10; col++) {
                    var divId = 'cell_' + row + '_' + col;
                    var imgId = 'img_' + row + '_' + col;
                    var divTag = '<div id="' + divId + '" name="brick" class="colorChip clearfix"></div>';
                    $(divTag).appendTo('.board');
                }
                $('<div class="clear">').appendTo('.board');
                $('</div>').appendTo('.board');
            }

            for (var row = 0; row < 2; row++) {
                for (var col = 0; col < 4; col++) {
                    var divId = 'next_' + row + '_' + col;
                    var imgId = 'nextImg_' + row + '_' + col;
                    var divTag = '<div id="' + divId + '" name="brick" class="colorChip clearfix"></div>';
                    $(divTag).appendTo('#divNext');
                }
                $('<div class="clear">').appendTo('#divNext');
                $('</div>').appendTo('#divNext');
            }
        }

        function setImages() {
            $('img[class="image"]').each(function (idx, el) {
                $('#' + el.id).attr('src', '../../Content/images/None.png');
            });
        }

        function showSplashScreen() {
            $('.subTitle').css('visibility', 'hidden');
            $('.press').css('visibility', 'hidden');
            $('.title').animate({
                top: 200
            }, 1000, 'swing', function () {
                // Animation complete.
                $('.subTitle').css('visibility', 'visible');
                $('.press').css('visibility', 'visible');
            });
        }

        function setTimer() {
            $(document).everyTime(1000, function (i) {

                $.ajax({
                    type: "GET",
                    url: "Tick",
                    cache: false,
                    dataType: "json",
                    error: function (xhr, status, error) {
                        //                        alert(xhr.status);
                    },
                    success: function (json) {
                    }
                });
            });

            $(document).everyTime(200, function (i) {

                if (gameState.current_state == 'playing') {

                    $.ajax({
                        type: "GET",
                        url: "GetBoard",
                        cache: false,
                        //                    data: {},
                        dataType: "json",
                        error: function (xhr, status, error) {
                            // you may need to handle me if the json is invalid
                            // this is the ajax object
                            //                        alert(xhr.status);
                            //                        alert(error);
                        },
                        success: function (json) {

                            if (json.IsGameOver) {
                                gameState.process('end');
                            }
                            else {
                                $('#divScore').text(json.Score);
                                $('#divHiScore').text(json.HiScore);
                                $('#divLines').text(json.Lines);
                                $('#divLevel').text(json.Level);

                                $.each(json.Bricks, function (i, val) {
                                    //                                    $('#img_' + val.Row + '_' + val.Col).attr('src', '../../Content/images/' + val.Color + '.png');
                                    $('#cell_' + val.Row + '_' + val.Col).css('background-image', '-webkit-gradient(linear, left top, right bottom, color-stop(0.0, ' + val.Color + '), color-stop(1.0, rgba(0, 0, 0, 0.0)))');
                                    $('#cell_' + val.Row + '_' + val.Col).css('border-color', val.Color);
                                });

                                for (var row = 0; row < 2; row++) {
                                    for (var col = 0; col < 4; col++) {
                                        $('#next_' + row + '_' + col).css('background-image', '-webkit-gradient(linear, left top, right bottom, color-stop(0.0, #000), color-stop(1.0, #000))');
                                        $('#next_' + row + '_' + col).css('border-color', '#333');
                                    }
                                }

                                $.each(json.Next, function (i, val) {
                                    $('#next_' + val.Row + '_' + val.Col).css('background-image', '-webkit-gradient(linear, left top, right bottom, color-stop(0.0, ' + val.Color + '), color-stop(1.0, rgba(0, 0, 0, 0.0)))');
                                    $('#next_' + val.Row + '_' + val.Col).css('border-color', val.Color);
                                });
                            }
                        }
                    });
                }
            });

            $(document).keydown(function (event) {
                switch (event.keyCode) {
                    case 32: //space
                        if (gameState.current_state == 'intro')
                            gameState.process('play');
                        else if (gameState.current_state == 'paused')
                            gameState.process('continue');
                        else if (gameState.current_state == 'gameOver')
                            gameState.process('showIntro');
                        else
                            gameState.process('pause');
                        break;
                    case 37: //left
                        if (gameState.current_state == 'playing')
                            moveLeft();
                        break;
                    case 38: //up
                        if (gameState.current_state == 'playing')
                            moveUp();
                        break;
                    case 39: //right
                        if (gameState.current_state == 'playing')
                            moveRight();
                        break;
                    case 40: //down
                        if (gameState.current_state == 'playing')
                            moveDown();
                        break;
                }
            });
        }

        function moveLeft() {
            $.ajax({
                type: "GET",
                url: "MoveLeft",
                cache: false,
                dataType: "json",
                error: function (xhr, status, error) {
                    //                    alert(xhr.status);
                },
                success: function (json) {
                }
            });
        }

        function moveRight() {
            $.ajax({
                type: "GET",
                url: "MoveRight",
                cache: false,
                dataType: "json",
                error: function (xhr, status, error) {
                    //                    alert(xhr.status);
                },
                success: function (json) {
                }
            });
        }

        function moveUp() {
            $.ajax({
                type: "GET",
                url: "MoveUp",
                cache: false,
                dataType: "json",
                error: function (xhr, status, error) {
                    //                    alert(xhr.status);
                },
                success: function (json) {
                }
            });
        }

        function moveDown() {
            $.ajax({
                type: "GET",
                url: "MoveDown",
                cache: false,
                dataType: "json",
                error: function (xhr, status, error) {
                    //                    alert(xhr.status);
                },
                success: function (json) {
                }
            });
        }

        function initializeBoard() {
            $.ajax({
                type: "GET",
                url: "InitializeBoard",
                cache: false,
                dataType: "json",
                error: function (xhr, status, error) {
                                        alert(xhr.status);
                },
                success: function (json) {
                }
            });
        }
    </script>
</head>
<body>
    <br />
    <div class="screen">
        <div id="title" class="title">
            <img src="../../Content/images/Title.png" />
            <div class="subTitle">©2011 Marcelo Ricardo de Oliveira<br />
            Made for The Code Project<img src="../../Content/images/Bob.png" class="bob"/></div>
            <br />
            <div class="press">Press SPACE to start game!</div>
        </div>
        <div class="centerPanel">
            <div class="board">
            </div>
            <div class="scorePanel">
                <div>
                    Score</div>
                    <div id="divScore" class="scoreText">000000</div>
                    <br />
                <div>
                    HiScore</div>
                    <div id="divHiScore" class="scoreText">000000</div>
                    <br />
                <div>
                    Lines</div>
                    <div id="divLines" class="scoreText">0</div>
                    <br />
                <div>
                    Level</div>
                    <div id="divLevel" class="scoreText">0</div>
                    <br />
                <div>
                    Next</div>
                    <div id="divNext" class="scoreText"></div>
                    
            </div>
        </div>
        <div id="gamePaused">
            GAME PAUSED<br />Press SPACE to continue!</div>
        </div>
        <div id="gameOver">
            GAME OVER<br />Press SPACE to restart!</div>
        </div>
    </div>
</body>
</html>
