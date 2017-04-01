(function(){
    "use strict";

    angular.module("studentsManagement").controller("StudentEditCtrl",
    ["student", "$state", "studentService", StudentEditCtrl]);

    function StudentEditCtrl(student, $state, studentService)
    {
        var vm = this;

        vm.student = student;

        if(vm.student && vm.student.RollNo)
        {
            vm.title = "Edit:" + vm.student.Name;
        }
        else{
            alert(student)
            vm.title = "New Student";
        }

        vm.opened = false;

        vm.open = function($event)
        {
            $event.preventDefault();
            $event.stopPropagation();

            vm.opened = ! vm.opened;
        }

        vm.getTotalMarks = function () { return studentService.getTotalMarks(vm.student)};
        vm.getAverage = function () { return studentService.getAverage(vm.student)};
        vm.getResult = function() {return studentService.getResult(vm.student)};


        vm.submit = function(isValid){
            if(isValid)
            {
            vm.student.$save(function(data)
            {
                toastr.success("Save successful");
            })}
            else{
                toastr.error("Please fix all the validation errors before saving it.")
            }
        }

        vm.cancel = function()
        {
            $state.go("studentsList");
        }

        vm.addRoles = function()
        {
            if(vm.newRoles)
            {
                var array = vm.newRoles.split(",");
                vm.student.Roles = vm.student.Roles ? vm.student.Roles.concat(array) : array;
                vm.newRoles = "";
            }
            else
            {
                alert("Enter at least one Role to add");
            }
        };

        vm.removeRole = function(idx)
        {
            vm.student.Roles.splice(idx,1);
        }
    }
}());