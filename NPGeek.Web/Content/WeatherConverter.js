function WeatherConversion() {

    var temp = document.cookie;

    if (temp == null || temp == "") {
        ToCelcius();
        document.cookie = "celcius";
    }
    else if (temp == "fahrenheit") {
        ToCelcius();
        document.cookie = "celcius";
    }
    else {
        ToFahrenheit();
        document.cookie = "fahrenheit";
    }
}



function ToCelcius() {

    var weather = document.getElementsByClassName('weather-conversion');

    for (var i = 0; i < weather.length; i++) {
        weather[i].textContent = Math.round((weather[i].textContent - 32) * (5 / 9));
    }
}

function ToFahrenheit() {

    var weather = document.getElementsByClassName('weather-conversion');

    for (var i = 0; i < weather.length; i++) {
        weather[i].textContent = Math.round((weather[i].textContent * 1.8) + 32);
    }
}
document.onload = Load();

function Load(){
    if (document.cookie == "celcius") {
        ToCelcius();
    }
}