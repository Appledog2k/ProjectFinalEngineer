const container = document.querySelector(".container-ex");
const search = document.querySelector(".search-box button");
const weatherBox = document.querySelector(".weather-box");
const weatherDetails = document.querySelector(".weather-details");
const error404 = document.querySelector(".not-found");
const notLocation = document.querySelector(".not-location");

search.addEventListener('click', () => {

    const APIKey = 'f509b0ac71453b1d13125bb4e2368303';
    const city = document.querySelector('.search-box input').value;

    if (city === '') {
        container.style.height = '100px';
        weatherBox.style.display = 'none';
        weatherDetails.style.display = 'none';
        notLocation.style.display = 'block';
        notLocation.classList.add('fadeIn');
        return;
    }


    fetch(`https://api.openweathermap.org/data/2.5/weather?q=${city}&units=metric&appid=${APIKey}`)
        .then(response => response.json())
        .then(json => {

            if (json.cod === '404') {
                container.style.height = '150px';
                weatherBox.style.display = 'none';
                weatherDetails.style.display = 'none';
                error404.style.display = 'block';
                error404.classList.add('fadeIn');
                return;
            }

            error404.style.display = 'none';
            error404.classList.remove('fadeIn');

            const image = document.querySelector('.weather-box img');
            const temperature = document.querySelector('.weather-box .temperature');
            const description = document.querySelector('.weather-box .description');
            const humidity = document.querySelector('.weather-details .humidity span');
            const wind = document.querySelector('.weather-details .wind span');

            switch (json.weather[0].main) {
                case "Clear":
                    image.src = "/images/avatars/clear.png";
                    break;

                case "Rain":
                    image.src = "/images/avatars/rain.png";
                    break;

                case "Snow":
                    image.src = "/images/avatars/snow.png";
                    break;

                case "Clouds":
                    image.src = "/images/avatars/cloud.png";
                    break;

                case "Haze":
                    image.src = "/images/avatars/mist.png";
                    break;

                default:
                    image.src = "";
            }

            temperature.innerHTML = `${parseInt(json.main.temp)}<span>°C</span>`;
            description.innerHTML = `${json.weather[0].description}`;
            humidity.innerHTML = `${json.main.humidity}%`;
            wind.innerHTML = `${parseInt(json.wind.speed)}Km/h`;

            weatherBox.style.display = '';
            weatherDetails.style.display = '';
            weatherBox.classList.add('fadeIn');
            weatherDetails.classList.add('fadeIn');
            container.style.height = '240px';


        });


});

$("#form").submit(function (e) {
    e.preventDefault()
    var query = $("#search").val()
    var API_KEY = 'AIzaSyApAutjcG0WckmOI-uQoIaVlve1dO1Ce4E'
    let result = ''
    var url = 'https://www.googleapis.com/customsearch/v1?key=' + API_KEY + "&cx=309416ca1ba204c6b&q=" + query + "&num=3"
    console.log(url);
    $.get(url, function (data) {
        $("#result").html('')
        console.log(data)
        data.items.forEach(item => {
            var title = item.title
            var snippet = item.snippet
            var link = item.link
            var result = `
                <div class="gg-result">
                    <h4><a target="_blank" href="${link}">${title}</a></h4>
                    <p>${snippet}</p>
                </div>
            `
            $("#result").append(result)
        });
    })
})