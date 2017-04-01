// JavaScript source code

(function() {
    "use strict";
    angular
        .module("studentsManagement")
        .controller("StudentListCtrl", ["studentResource", StudentListCtrl]);

    function StudentListCtrl(studentResource)
    {
        var vm = this;

        studentResource.query(function(data) {
            vm.students = data;
        });

//vm.students = [
//        {
//            "RollNo": "1",
//            "Name": "Sumanesh",
//            "Age": 34,
//            "Image": "Images/Man.png"
//        },
//        {
//            "RollNo": "2",
//            "Name": "Saveetha",
//            "Age": 31,
//            "Image": "Images/Woman.jpg"
//        },
//        {
//            "RollNo": "3",
//            "Name": "Aadhavan",
//            "Age": 4,
//            "Image": "Images/Boy.png"
//        },
//        {
//            "RollNo": "4",
//            "Name": "Aghilan",
//            "Age": 0.3,
//            "Image": "Images/Baby.jpg"
//        }
//    ];

        vm.showImage = false;

        vm.toggleImage = function()
        {
            vm.showImage = !vm.showImage;
        }
    }
}());