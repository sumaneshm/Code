(function()
{
   "use strict";

    angular.module("studentsManagement")
        .controller("StudentDetailCtrl",["student","studentService",StudentDetailCtrl]);

    function StudentDetailCtrl(student, studentService)
    {
        var vm = this;


        vm.student = student;

        vm.title="Student detail :" + vm.student.Name;

        vm.Fee = studentService.calculateFee(vm.student);
        vm.Salutation = studentService.getSalutation(vm.student);
        vm.getTotalMarks = function () { return studentService.getTotalMarks(vm.student)};
        vm.getAverage = function () { return studentService.getAverage(vm.student)};
        vm.getResult = function() {return studentService.getResult(vm.student)};


        if(vm.student.Tags)
        {
            vm.student.tagList = vm.student.Tags.toString();
        }
    }
}());