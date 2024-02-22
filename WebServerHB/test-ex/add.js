if(!localStorage.getItem("user_id")){
    localStorage.setItem("user_id", uuid())
}

function uuid() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

let inputTitle = document.getElementById('input_title');
let inputCategory = document.getElementById('input_category');
let inputText = document.getElementById('input_data');
let errorTitle = document.getElementById('error_title');
let errorCategory = document.getElementById('error_category');
let errorText = document.getElementById('error_date')
let categoryOptions = document.querySelector('#input_category_options');
let buttonAdd = document.getElementById('button_add');

categoryOptions.addEventListener('click', selectOption);
function selectOption(event){
    inputCategory.textContent = event.target.textContent;
}

buttonAdd.addEventListener("click", function() {

    const outputData = { Title: inputTitle.value ?? " ",
        Category: inputCategory.textContent ?? " ",
        Date: inputText.value ?? " " };

    $.post('http://localhost:5000/add-date', JSON.stringify(outputData), "json")
        .done(function (response){

            if(response.Error === "Вы не авторизованы") {
                alert(response.Error);
                return;
            }

            errorTitle.textContent = response.Title;
            errorCategory.textContent = response.Category;
            errorText.textContent = response.Text;
            if(errorText.textContent === '' && errorCategory.textContent === '' && errorText.textContent === ''){
                alert('Ваш запрос отправлен!');
            }

        });
});

document.getElementById('navigation_home').onclick = function () {
    window.location = 'http://localhost:5000/home';
}
document.getElementById('navigation_list').onclick = function () {
    window.location = 'http://localhost:5000/list';
}