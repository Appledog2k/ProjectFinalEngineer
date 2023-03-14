const container = document.querySelector(".container-ex");
const container1 = document.querySelector(".container-ex-1");
const imageGoogle = document.querySelector(".image-google");
const titleGoogle = document.querySelector(".container-ex-1  .wt-title");
const search = document.querySelector(".search-box button");
const weatherBox = document.querySelector(".weather-box");
const weatherDetails = document.querySelector(".weather-details");
const error404 = document.querySelector(".not-found");
const notLocation = document.querySelector(".not-location");

$(document).ready(function () {
    navbarActive();
    openMenu();
});
function openMenu() {
    $("#menu-icon").click(function () {
        $(this).toggleClass("bx-x");
        $(".navbar").toggleClass("open");
    });
}
function navbarActive() {
    $(this).on("click", "ul li", function () {
        $(this).addClass("active").siblings().removeClass("active");
    });
}


search.addEventListener("click", () => {

    const APIKey = "f509b0ac71453b1d13125bb4e2368303";
    const city = document.querySelector(".search-box input").value;

    if (city === "") {
        container.style.height = "100px";
        weatherBox.style.display = "none";
        weatherDetails.style.display = "none";
        notLocation.style.display = "block";
        notLocation.classList.add("fadeIn");
        return;
    }


    fetch(`https://api.openweathermap.org/data/2.5/weather?q=${city}&units=metric&appid=${APIKey}`)
        .then(response => response.json())
        .then(json => {

            if (json.cod === "404") {
                container.style.height = "150px";
                weatherBox.style.display = "none";
                weatherDetails.style.display = "none";
                error404.style.display = "block";
                error404.classList.add("fadeIn");
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
                case 'Clear':
                    image.src = '/images/avatars/clear.png';
                    break;

                case 'Rain':
                    image.src = '/images/avatars/rain.png';
                    break;

                case 'Snow':
                    image.src = '/images/avatars/snow.png';
                    break;

                case 'Clouds':
                    image.src = '/images/avatars/cloud.png';
                    break;

                case 'Haze':
                    image.src = '/images/avatars/mist.png';
                    break;

                default:
                    image.src = '';
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
    if (query === '') {
        var result1 = `
                <div class="gg-result">
                    <p style="font-size:13px color:white;">Vui lòng nhập từ khóa để tìm kiếm</p>
                </div>
            `
        $("#result").append(result1)
        container1.style.height = '100px';
        imageGoogle.style.display = 'none';
        return;
    }
    var API_KEY = 'AIzaSyApAutjcG0WckmOI-uQoIaVlve1dO1Ce4E'
    let result = ''
    var url = 'https://www.googleapis.com/customsearch/v1?key=' + API_KEY + "&cx=309416ca1ba204c6b&q=" + query +"&num=3"
    console.log(url);
    $.get(url, function (data) {
        $("#result").html('')
        console.log(data)
        if (data.items == null) {
            var result =
                `
            <div class="gg-result">
                <h4>Không tìm thấy kết quả mong đợi</h4>
            </div>
            `
            $("#result").append(result)
            container1.style.height = '100px';
            imageGoogle.style.display = 'none';
        } else {
            data.items.forEach(item => {
                var title = item.title
                var snippet = item.snippet
                var link = item.link
                var result = `
                <div class="gg-result">
                    <h4><a target="_blank" href="${link}" style="font-size:13px; color: white;">${title}</a></h4>
                    <p style="font-size:11px">${snippet}</p>
                </div>
            `
                $("#result").append(result)
                container1.style.height = 'auto';
                container1.style.background = "rgb(25 48 78)";
                imageGoogle.style.display = "none";
                titleGoogle.style.display = 'none';
            });
        }
       
    })
})