# BNO055-Unity

Unity plugin to work with [Adafruit BNO055 Absolute Orientation Sensor](https://learn.adafruit.com/adafruit-bno055-absolute-orientation-sensor/overview).

## Prerequirement

You need to compile and upload the [bunny sketch](https://github.com/adafruit/Adafruit_BNO055/blob/master/examples/bunny/bunny.ino) to your Arduino included in [Adafruit's BNO055 repository](https://github.com/adafruit/Adafruit_BNO055).

## Get started with an example

1. Open Example/example.unity

2. Replace BNO055's "SerialReceiver"->"Port Name" property with the path to your Arduino.

3. Run the scene.

## Get started with your scene.

1. Import [bno055-unity.unitypackage](bno055-unity.unitypackage)

2. Attach Scripts/Serial Receiver and Scripts/Tracker to the GameObject. That GameObject rotates as BNO055 moves.

## Demo

![Demo](demo.gif)

## License 

MIT License.
