var grd = function () {
    let firstStar = $('#firstStar');
    let secondStar = $('#secondStar');
    let thirdStar = $('#thirdStar');
    let fourthStar = $('#fourthStar');
    let fifthStar = $('#fifthStar');
    let formToFill = $('#stars');
   
    firstStar.click(function () {
        secondStar.css('color','#007bff');
        thirdStar.css('color', '#007bff');
        fourthStar.css('color','#007bff');
        fifthStar.css('color', '#007bff');

        firstStar.css('color', '#fbd600');
        formToFill.val(1);
    });

    secondStar.click(function () {
        firstStar.css('color', '#007bff');
        secondStar.css('color', '#007bff');
        thirdStar.css('color', '#007bff');
        fourthStar.css('color', '#007bff');
        fifthStar.css('color', '#007bff');

        firstStar.css('color', '#fbd600');
        secondStar.css('color', '#fbd600');
        formToFill.val(2);
    });

    thirdStar.click(function () {
        firstStar.css('color', '#007bff');
        secondStar.css('color', '#007bff');
        thirdStar.css('color', '#007bff');
        fourthStar.css('color', '#007bff');
        fifthStar.css('color', '#007bff');

        firstStar.css('color', '#fbd600');
        secondStar.css('color', '#fbd600');
        thirdStar.css('color', '#fbd600');
        formToFill.val(3);
    });

    fourthStar.click(function () {
        firstStar.css('color', '#007bff');
        secondStar.css('color', '#007bff');
        thirdStar.css('color', '#007bff');
        fourthStar.css('color', '#007bff');
        fifthStar.css('color', '#007bff');

        firstStar.css('color', '#fbd600');
        secondStar.css('color', '#fbd600');
        thirdStar.css('color', '#fbd600');
        fourthStar.css('color', '#fbd600');
        formToFill.val(4);
    });

    fifthStar.click(function () {
        firstStar.css('color', '#007bff');
        secondStar.css('color', '#007bff');
        thirdStar.css('color', '#007bff');
        fourthStar.css('color', '#007bff');
        fifthStar.css('color', '#007bff');

        firstStar.css('color', '#fbd600');
        secondStar.css('color', '#fbd600');
        thirdStar.css('color', '#fbd600');
        fourthStar.css('color', '#fbd600');
        fifthStar.css('color', '#fbd600');
        formToFill.val(5);
    });
};

grd('1');