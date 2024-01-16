class Player extends PIXI.Sprite{
    constructor(x=0,y=0){
        super(app.loader.resources["images/player.png"].texture);
        this.anchor.set(.5,.5);
        this.scale.set(0.4);
        this.x = x;
        this.y = y;
        this.vx = 0;
        this.vy = 0;
        this.speed = 5;
    }
}

class Enemy extends PIXI.Sprite{
    constructor(x=0,y=0){
        super(app.loader.resources["images/enemy.png"].texture);
        this.anchor.set(.5,.5);
        this.scale.set(0.5);
        this.x = x;
        this.y = y;

        this.fwd = getRandomUnitVector();

        this.speed = 50;
        this.isAlive = true;
    }

    move(dt=1/60){
        this.x += this.fwd.x * this.speed * dt;
        this.y += this.fwd.y * this.speed * dt;
    }

    reflectX(){
        this.fwd.x *= -1;
    }

    reflectY(){
        this.fwd.y *= -1;
    }
}

class Slime extends PIXI.Sprite{
    constructor(x=0,y=0){
        super(app.loader.resources["images/slime.png"].texture);
        this.anchor.set(.5,.5);
        this.scale.set(0.1);
        this.x = x;
        this.y = y;

        this.isAlive = true;
    }
}

class Bullet extends PIXI.Graphics{
    constructor(color=0x006400, x=0, y=0){
        super();
        this.beginFill(color);
        this.drawRect(-2,-3,7,10);
        this.endFill();
        this.x = x;
        this.y = y;
        this.angle = 0;

        this.fwd = {x:0,y:-1};
        this.speed = 10;
        this.isAlive = true;
        Object.seal(this);
    }
}