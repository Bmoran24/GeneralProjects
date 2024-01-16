"use strict";
const app = new PIXI.Application({
    width: 1000,
    height: 700,
    backgroundColor: 0xFBBF77
});

document.body.appendChild(app.view);

const sceneWidth = app.view.width;
const sceneHeight = app.view.height;

// Image Pre-load
app.loader.add([
    "images/player.png",
    "images/enemy.png",
    "images/slime.png"
]);

app.loader.onProgress.add(e => {console.log(`progress=${e.progress}`)});
app.loader.onComplete.add(setup);
app.loader.load();

let stage;

let startScene;
let gameScene, player, scoreLabel, healthLabel, bulletLabel, levelLabel, shootSound, slimeSound, hurtSound;
let gameOverScene;
let gameOverScoreLabel;

let enemies = [];
let bullets = [];
let slime = [];
let bulletTimer = 0;
let bulletCount = 0;
let score = 0;
let health = 100;
let level = 1;
let paused = true;
let slimeSpawnRate = 5;

function setup() {
    stage = app.stage;

    startScene = new PIXI.Container();
    stage.addChild(startScene);

    gameScene = new PIXI.Container();
    gameScene.visible = false;
    stage.addChild(gameScene);

    gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);

    createTextAndButtons();

    player = new Player();
    gameScene.addChild(player);

    shootSound = new Howl({
        src: ['sounds/shoot.mp3']
    });

    slimeSound = new Howl({
        src: ['sounds/slime.mp3']
    });

    hurtSound = new Howl({
        src: ['sounds/death.mp3']
    });

    app.ticker.add(gameLoop);

    app.view.onClick = fireBullet;
}

function createTextAndButtons(){
    // Styles & Label Setup
    // - Start Scene
    let buttonStyle = new PIXI.TextStyle({
        fill: 0xADD8E6, // blue
        stroke: 0x00008B,
        strokeThickness: 8,
        fontSize: 60,
        fontFamily: "Verdura"
    });

    let startTitleLabel = new PIXI.Text("MONSTER SMASH");
    startTitleLabel.style = new PIXI.TextStyle({
        fill: 0x013220, // dark green
        fontSize: 100,
        fontFamily: "Verdura",
        dropShadow: true,
        stroke: 0x90EE90, // light green
        strokeThickness: 10
    });

    startTitleLabel.x = 80;
    startTitleLabel.y = 150;
    startScene.addChild(startTitleLabel);

    let directionsLabel = new PIXI.Text("Use WASD to Avoid Enemy Monsters and the \nMouse Pointer to Launch the Slime You Collect!");
    directionsLabel.style = new PIXI.TextStyle({
        fill: 0x013220,
        fontSize: 40,
        fontFamily: "Verdura"
    });

    directionsLabel.x = sceneWidth/10;
    directionsLabel.y = sceneHeight/2;
    
    startScene.addChild(directionsLabel);

    let startButton = new PIXI.Text("START");
    startButton.style = buttonStyle;
    startButton.x = 400;
    startButton.y = sceneHeight - 100;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", startGame);
    startButton.on('pointerover', e=>e.target.alpha=0.7);
    startButton.on('pointerout', e=>e.currentTarget.alpha=1.0);
    startScene.addChild(startButton);

    // - Game Scene

    let textStyle = new PIXI.TextStyle({
        fill: 0xFFFFFF, // white
        fontSize: 18,
        fontFamily: "Verdura",
        stroke: 0x000000,
        strokeThickness: 2
    });

    scoreLabel = new PIXI.Text();
    scoreLabel.style = textStyle;
    scoreLabel.x = 5;
    scoreLabel.y = 5;
    gameScene.addChild(scoreLabel);
    increaseScoreBy(0);

    healthLabel = new PIXI.Text();
    healthLabel.style = textStyle;
    healthLabel.x = 5;
    healthLabel.y = 26;
    gameScene.addChild(healthLabel);
    decreaseHealthBy(0);

    bulletLabel = new PIXI.Text();
    bulletLabel.style = textStyle;
    bulletLabel.x = sceneWidth - 100;
    bulletLabel.y = 5;
    gameScene.addChild(bulletLabel);
    bulletLabel.text = `Ammo: ${bulletCount}`;

    levelLabel = new PIXI.Text();
    levelLabel.style = textStyle;
    levelLabel.x = sceneWidth - 100;
    levelLabel.y = 26;
    gameScene.addChild(levelLabel);
    levelLabel.text = `Level: ${level}`;

    // - Game Over Scene
    let gameOverText = new PIXI.Text("GAME OVER");
    textStyle = new PIXI.TextStyle({
        fill: 0xFF0000, // red
        fontSize: 64,
        fontFamily: "Verdura",
        stroke: 0xFFFFFF, 
        strokeThickness: 10
    });

    gameOverText.style = textStyle;
    gameOverText.x = sceneWidth/3;
    gameOverText.y = sceneHeight/2 - 160;
    gameOverScene.addChild(gameOverText);

    gameOverScoreLabel = new PIXI.Text("Your Final Score: ");
    gameOverScoreLabel.style = textStyle;
    gameOverScoreLabel.x = sceneWidth/3 -50;
    gameOverScoreLabel.y = sceneHeight/2 + 20;
    gameOverScene.addChild(gameOverScoreLabel);

    let playAgainButton = new PIXI.Text("Play Again?");
    playAgainButton.style = buttonStyle;
    playAgainButton.x = sceneWidth/3 + 50;
    playAgainButton.y = sceneHeight - 100;
    playAgainButton.interactive = true;
    playAgainButton.buttonMode= true;
    playAgainButton.on("pointerup", startGame);
    playAgainButton.on('pointerover',e=>e.target.alpha=0.7);
    playAgainButton.on('pointerout', e=>e.currentTarget.alpha = 1.0);
    gameOverScene.addChild(playAgainButton);
}

function increaseScoreBy(value){
    score += value;
    scoreLabel.text = `Score: ${score}`;
}

function decreaseHealthBy(value){
    health -= value;
    healthLabel.text = `Health: ${health}%`;
}

function updateBulletCount(){
    bulletLabel.text = `Ammo: ${bulletCount}`;
}

function pickupBullet(){
    bulletCount += 1;
    bulletLabel.text = `Ammo: ${bulletCount}`;
    slimeSound.play();
}

function increaseLevel(){
    level += 1;
    levelLabel.text = `Level: ${level}`;
    if(slimeSpawnRate>=2)
    {
        slimeSpawnRate-=0.75;
    }
}

function startGame(){
    startScene.visible = false;
    gameOverScene.visible= false;
    gameScene.visible = true;
    level = 1;
    score = 0;
    health = 100;
    bulletCount = 0;
    increaseScoreBy(0);
    decreaseHealthBy(0);
    player.x = 400;
    player.y = 600;
    bulletLabel.text = `Ammo: ${bulletCount}`;
    scoreLabel.text = `Score: ${score}`;
    levelLabel.text = `Level: ${level}`;
    gameOverScoreLabel.text = `Your Final Score: `;
    slimeSpawnRate = 5;
    loadLevel();
}

function spawnAmmo(numAmmo){
    for(let i=0; i<numAmmo; i++)
    {
        let s = new Slime(getRandom(0, sceneWidth), getRandom(100, sceneHeight));
        slime.push(s);
        gameScene.addChild(s);
    }
}


function spawnEnemies(numEnemies){
    for(let i=0; i<numEnemies; i++)
    {
        let e = new Enemy(getRandom(100, sceneWidth-100), getRandom(25, 75));
        enemies.push(e);
        gameScene.addChild(e);
    }
}

function loadLevel(){
    spawnEnemies(3 * level);
    spawnAmmo(3);
    paused = false;
}

function gameLoop(){
    if(paused) return;

    let dt = 1/app.ticker.FPS;
    if(dt > 1/12) dt =1/12;

    bulletTimer += dt;

    if(bulletTimer >= slimeSpawnRate && bulletCount < 5)
    {
        spawnAmmo(1);
        bulletTimer = 0;
    }

    player.x += player.vx;
    player.y += player.vy;

    if(player.x <= 0)
    {
        player.x = 0;
    }
    else if(player.x >= sceneWidth)
    {
        player.x = sceneWidth;
    }

    if(player.y <= 0)
    {
        player.y = 0;
    }
    else if(player.y >= sceneHeight)
    {
        player.y = sceneHeight;
    }

    seekPlayer();

    // Move Enemies
    for(let e of enemies)
    {
        e.move(dt);
        if(e.x <= 0)
        {
            e.x = 0;
        }
        else if(e.x >= sceneWidth)
        {
            e.x = sceneWidth;
        }

        if(e.y <= 0)
        {
            e.y = 0;
        }
        else if(e.y >= sceneHeight)
        {
            e.y = sceneHeight;
        }
    }

    // Move Bullets
    for(let b of bullets)
    {
        let vx = b.speed * Math.cos(b.angle);
        let vy = b.speed * Math.sin(b.angle);

        b.x += vx;
        b.y += vy;
    }

    // Collision Detection
    for(let e of enemies){
        for(let b of bullets){
            if(rectsIntersect(e,b)){
                gameScene.removeChild(e);
                e.isAlive = false;
                gameScene.removeChild(b);
                b.isAlive = false;
                increaseScoreBy(1);
            }

            if(b.y <= -10) b.isAlive = false;
        }

        if(e.isAlive && rectsIntersect(e, player)){
            gameScene.removeChild(e);
            e.isAlive = false;
            decreaseHealthBy(20);
            hurtSound.play();
        }
    }

    for(let s of slime)
    {
        if(rectsIntersect(player, s))
        {
            gameScene.removeChild(s);
            s.isAlive = false;
            pickupBullet();
        }
    }

    bullets = bullets.filter(b=>b.isAlive);

    slime = slime.filter(s=>s.isAlive);

    enemies = enemies.filter(e=>e.isAlive);

    updateBulletCount();

    if(health <= 0)
    {
        end();
        return;
    }

    if(enemies.length == 0)
    {
        increaseLevel();
        loadLevel();
    }
}

function seekPlayer(){
    for(let e of enemies)
    {
        let dx = player.x - e.x;
        let dy = player.y - e.y;
        let force = e.speed/50;

        if(dx < 0 && dy > 0)
        {
            e.x -= force;
            e.y += force;
        }
        else if(dx > 0 && dy > 0)
        {
            e.x += force;
            e.y += force;
        }
        else if(dx < 0 && dy < 0)
        {
            e.x -= force;
            e.y -= force;
        }
        else if(dx >0 && dy <0)
        {
            e.x += force;
            e.y -= force;
        }
        else if(dx == 0 && dy < 0)
        {
            e.y -= force;
        }
        else if(dx == 0 && dy > 0)
        {
            e.y += force;
        }
        else if(dy == 0 && dx < 0)
        {
            e.x -= force;
        }
        else if(dy == 0 && dx >0)
        {
            e.x += force;
        }

    }
}

// Player Input Control
window.addEventListener('keydown', (e) => {
    // Update player velocity based on key pressed
    switch (e.key) {
        case 'a':
            player.vx = -player.speed;
            break;
        case 'd':
            player.vx = player.speed;
            break;
        case 'w':
            player.vy = -player.speed;
            break;
        case 's':
            player.vy = player.speed;
            break;
    }
})

window.addEventListener('click', ()=>{
    if(bulletCount>0) fireBullet();    
})

window.addEventListener('keyup', e=> {
    // Reset player velocity to 0 when keys are released
    switch(e.key) {
        case 'a':
            player.vx = 0;
            break;
        case 'd':
            player.vx = 0;
            break;
        case 'w':
            player.vy = 0;
            break;
        case 's':
            player.vy = 0;
            break;
    }
})

function fireBullet(){
    if(paused) return;

    let mousePosition = app.renderer.plugins.interaction.mouse.global;

    let b = new Bullet(0x006400, player.x, player.y);

    b.angle = Math.atan2(mousePosition.y - player.y, mousePosition.x - player.x);

    bullets.push(b);
    gameScene.addChild(b);

    shootSound.play();

    bulletCount--;
}

function end(){
    paused = true;

    enemies.forEach(c=>gameScene.removeChild(c));
    enemies = [];

    bullets.forEach(b=>gameScene.removeChild(b));
    bullets = [];

    slime.forEach(s=>gameScene.removeChild(s));
    slime = [];

    gameOverScoreLabel.text += score;

    gameOverScene.visible = true;
    gameScene.visible = false;
}