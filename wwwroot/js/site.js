// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll(".check-button").forEach(button => {
    button.addEventListener("click", function () {
        let img = this.querySelector("img");
        this.classList.toggle("checked");

        img.src = this.classList.contains("checked") ? "/images/check.svg" : "/images/plus1.svg";
        img.alt = this.classList.contains("checked") ? "check button" : "plus button";
    });
});