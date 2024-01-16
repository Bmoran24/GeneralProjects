function rectsIntersect(a,b){
    let ab = a.getBounds();
    let bb = b.getBounds();

    if( ab.x + ab.width > bb.x && ab.x < bb.x + bb.width && ab.y + ab.height > bb.y && ab.y < bb.y + bb.height)
    {
        return true;
    }
    
    return false;
    
}

function lerp(start, end, amt){
    return start * (1-amt) + amt * end;
}

function clamp(val, min, max){
    return val < min ? min : (val > max ? max : val);
}

function getRandomUnitVector(){
    let x = getRandom(-1,1);
    let y = getRandom(-1,1);
    let length = Math.sqrt(x*x + y*y);
    if(length == 0){ // very unlikely
        x=1; // point right
        y=0;
        length = 1;
    } else{
        x /= length;
        y /= length;
    }

    return {x:x, y:y};
}

function getRandom(min, max) {
    return Math.random() * (max-min) + min;
}