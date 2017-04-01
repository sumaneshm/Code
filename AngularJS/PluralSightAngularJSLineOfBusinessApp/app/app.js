// JavaScript source code

(function () {
    "use strict";

    var app = angular.module("studentsManagement", ["common.services", "ngMessages", "ui.router","ui.mask", "ui.bootstrap", "angularCharts", "studentResourceMock"]);

    app.filter('reverse', function() {
        return function(items) {
            return items.slice().reverse();
        };
    });

    app.filter('range', function() {
        return function(input, total) {
            total = parseInt(total);
            for (var i=0; i<total; i++)
                input.push(i);
            return input;
        };
    });

    app.config(["$stateProvider", "$urlRouterProvider",
            function($stateProvider, $urlRouterProvider) {

                $urlRouterProvider.otherwise("/");

                $stateProvider.state("home",{
                        url: "/",
                        templateUrl: "app/welcomeView.html"
                    }
                );

                $stateProvider.state("studentsList", {
                    url: "/students",
                    templateUrl:"app/students/studentListView.html",
                    controller: "StudentListCtrl as vm"
                });

                $stateProvider.state("studentEdit", {
                    abstract: true,
                    url: "/students/edit/:rollNo",
                    templateUrl:"app/students/studentEditView.html",
                    controller: "StudentEditCtrl as vm",
                    resolve: {
                        studentResource : "studentResource",

                        student : function(studentResource, $stateParams)
                        {

                            var rollNo = $stateParams.rollNo;
                            alert("Getting rollNo" + rollNo);
                            return studentResource.query({rollNo: rollNo}).$promise;
                        }

                    }
                });

                $stateProvider.state("studentEdit.info", {
                    url: "/info",
                    templateUrl: "app/students/StudentEditInfoView.html"
                });

                $stateProvider.state("studentEdit.marks", {
                    url: "/info",
                    templateUrl: "app/students/StudentEditMarksView.html"
                });

                $stateProvider.state("studentEdit.roles", {
                   url:"/roles",
                    templateUrl:"app/students/StudentEditRolesView.html"
                });

                $stateProvider.state("studentDetail",{
                    url: "/studentDetail/:rollNo",
                    templateUrl: "app/students/StudentDetailView.html",
                    controller: "StudentDetailCtrl as vm",
                    resolve: {
                        studentResource : "studentResource",

                        student : function(studentResource, $stateParams)
                        {

                            var rollNo = $stateParams.rollNo;
                            return studentResource.get({rollNo: rollNo}).$promise;
                        }

                    }
                });

                $stateProvider.state("marksAnalytics",{
                    url:"/marksAnalytics",
                    templateUrl:"app/marks/marksAnalyticsView.html",
                    controller: "MarksAnalyticsCtrl",
                    resolve:
                    {
                        studentResource : "studentResource",

                        students : function(studentResource)
                        {
                            return studentResource.query(function(response) {
                                    // no code needed for success
                                },
                                function(response) {
                                    if (response.status == 404) {
                                        alert("Error accessing resource: " +
                                        response.config.method + " " +response.config.url);
                                    } else {
                                        alert(response.statusText);
                                    }
                                }).$promise;
                        }
                    }
                })

    }]);


}());