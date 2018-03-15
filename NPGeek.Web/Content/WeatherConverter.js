function WeatherConversion() {

    var temp = document.cookie;    

    if (temp == "fahrenheit" || temp == null) {
        ToCelcius();
        document.cookie = "celcius";
    }
    else {
        ToFahrenheit();
        document.cookie = "fahrenheit";
    }
}



function ToFahrenheit() {

    var weather = document.getElementsByClassName('weather-conversion');

    for (var i = 0; i < weather.length; i++) {
        weather[i].textContent = Math.round((weather[i].textContent - 32) / 1.8);
    }
}

function ToCelcius() {

    var weather = document.getElementsByClassName('weather-conversion');

    for (var i = 0; i < weather.length; i++) {
        weather[i].textContent = Math.round((weather[i].textContent * 1.8) + 32);
    }
}