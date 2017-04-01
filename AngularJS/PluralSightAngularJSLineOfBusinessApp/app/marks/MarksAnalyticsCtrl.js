
(function ()
{
   "use strict";
    angular.module("studentsManagement")
        .controller("MarksAnalyticsCtrl",
                            ["$scope", "$filter","students", "studentService", MarksAnalyticsCtrl]);

    function MarksAnalyticsCtrl($scope,$filter, students, studentService) {
        $scope.title = "Marks Analytics";

        for (var i = 0; i < students.length; i++)
        {
            students[i].average = studentService.getAverage(students[i]);
            students[i].total = studentService.getTotalMarks(students[i]);
        }


        var studentsOrderedByAverage = $filter("orderBy")(students,"average");
        //var studentsOrderedByAverageReverse = $filter("reverse")(studentsOrderedByAverage,"average");
        var filteredStudentsAverage = $filter("limitTo")(studentsOrderedByAverage.reverse(),3);

        var chartDataAverage = [];
        for(var i=0;i<filteredStudentsAverage.length;i++)
        {
            chartDataAverage.push({
                x:filteredStudentsAverage[i].Name,
                y: [filteredStudentsAverage[i].total, filteredStudentsAverage[i].average]
            });
        }


        $scope.dataAverage = {
            series : ["Total","Average"],
            data: chartDataAverage
        };

        $scope.configAverage = {
            title: "Top student averages",
            tooltips : true,
            labels: false,
            mouseover:function (){},
            mouseout: function () {},
            click: function () {},
            legend:
            {
                display:true,
                position:'right'
            }
        }
    }
} ());