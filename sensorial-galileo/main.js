// --> requires 
//
var mraa = require('mraa');
var express = require('express');
var bodyParser = require('body-parser');
var path = require('path');
var mraa = require('mraa'); //require mraa
var groveSensor = require('jsupm_grove');


// --> light 
//
var myRelay = new groveSensor.GroveRelay(6);
var lightStatus = 0;
myRelay.off();
function toggleLight() {
    console.log('Light changed');
    lightStatus = !lightStatus;
    lightStatus ? myRelay.on() : myRelay.off();
};


// --> air flow
//
var myRelay2 = new groveSensor.GroveRelay(3);
myRelay2.off();
var airStatus = 0;
function toggleAir() {
    console.log('Air changed');
    airStatus = !airStatus;
    airStatus ? myRelay2.on() : myRelay2.off();
};

// --> smell
//
var myEnergy = new mraa.Gpio(8);
myEnergy.dir(mraa.DIR_OUT);
myEnergy.write(0);

function energyNivel1(){
    myEnergy.write(1);
    setTimeout(function () {
        myEnergy.write(0);
    }, 300);
}
function energyNivel2(){
    myEnergy.write(1);
    setTimeout(function () {
        myEnergy.write(0);
    }, 600);
}
function energyNivel3(){
    myEnergy.write(1);
    setTimeout(function () {
        myEnergy.write(0);
    }, 900);
}

function toggleSmell() {
    console.log('Smell changed');
    if(myEnergy.read() == 0) 
        energyNivel3();
};

// --> button
//
var myButton2 = new groveSensor.GroveButton(4);
setInterval(function () {
    if(myButton2.value()) {
        toggleLight();
        toggleAir();
        toggleSmell();
    }
}, 200);

// --> web server configure
//
var app = express();
var root = path.join(__dirname, './');
app.use(express.static(path.join(root, '/client')));
app.set('views', path.join(root, '/views'));
app.set('view engine', 'ejs');

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());


// --> web sever routes
//

// webpage route
var router = express.Router();
router.get('/', function(req, res) {
    res.render('index');
});
app.use('/', router);

// light route
var api1 = express.Router();
api1.get('/light', function(req, res) {
  console.log("light get");
  res.json({ message: lightStatus }).end();
});
api1.put('/light', function(req, res) {
  console.log("light put");
  toggleLight();
  res.json({ message: lightStatus }).end();
});
app.use('/api', api1);

// air route
var api2 = express.Router();
api2.get('/air', function(req, res) {
  console.log("light get");
  res.json({ message: airStatus }).end();
});
api2.put('/air', function(req, res) {
  console.log("light put");
  toggleAir();
  res.json({ message: airStatus }).end();
});
app.use('/api', api2);

// smell route
var api3 = express.Router();
api3.put('/smell', function(req, res) {
  toggleSmell();
});
app.use('/api', api3);

// --> web run
//
var port = process.env.PORT || 8080;
app.listen(port, function() {
    console.log("Listening on " + port);
});

