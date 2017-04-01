/**
 * Created by sumaneshm on 12/15/2014.
 */

(function() {
    "use strict";
    var app = angular.module("studentResourceMock",["ngMockE2E"]);



    app.run(function($httpBackend) {
        var students = [
            {
                "RollNo": "1",
                "Name": "Sumanesh",
                "Age": 34,
                "DOB": "December 19, 1980",
                "ID": "97CS28",
                "Roles":["Father","Husband"],
                //"Marks":[98,89,33,66,75],
                "Marks":[6,6,6],
                "Image": "Images/Man.png"
            },
            {
                "RollNo": "2",
                "Name": "Saveetha",
                "DOB": "March 16, 1982",
                "Age": 31,
                "Roles": ["Mother","Wife"],
                //"Marks":[95,85,83,88,70],
                "Marks":[2,2,2],
                "ID": "97CS01",
                "Image": "Images/Woman.jpg"
            },
            {
                "RollNo": "3",
                "Name": "Aadhavan",
                "DOB": "February 26, 2010",
                "Roles":["Son", "Elder brother"],
                "Age": 4,
              //  "Marks":[98,89,99,100,99],
                "Marks":[3,3,3],
                "ID": "97CS02",
                "Image": "Images/Boy.png"
            }
            ,{
                "RollNo": "4",
                "Name": "Aghilan",
                "Roles":["Son", "Younger brother"],
                "DOB": "October 22, 2014",
                "ID": "97CS03",
                "Age": 0.3,
                //"Marks":[99,81,100,100,99],
                "Marks":[4,4,4],
                "Image": "Images/Baby.jpg"
            }
            ,{
                "RollNo": "5",
                "Name": "Vennila",
                "Roles":["Daughter", "Sister"],
                "ID": "97CS04",
                //"Marks":[98,100,100,100,99],
                "Marks":[5,5,5],
                "DOB": "December 19, 2015",
                "Age": 0.1,
                "Image": "Images/Baby.jpg"
            }
        ];

        function getStudent(id)
        {


            if(id > 0)
            {

                for(var i=0;i<students.length;i++)
                {
                    if(students[i].RollNo == id)
                    {


                        return students[i];

                    }
                }
            }

            return null;
        }

        var studentsUrl = "/api/studentsRepository";


        $httpBackend.whenGET(studentsUrl).respond(students);

        $httpBackend.whenGET(
            new RegExp("api\/studentsRepository\?.*[0-9][0-9]*",'')
        ).respond(function(method, url, data)
            {
            var student = {"RollNo" : 0}
            var params = url.split("=");
            var length = params.length;
            var id = params[length-1];

            student = getStudent(id);

            return [200, student, {}];

        });

        $httpBackend.whenPOST(studentsUrl).respond(
            function(method,url,data)
            {
                var stud = angular.fromJson(data);
                if(!stud.RollNo)
                {

                    stud.RollNo = students[students.length - 1].RollNo + 1;
                    students[students.length] = stud;
                }
                else
                {
                    for(var i=0;i<students.length;i++)
                    {
                        if(students[i].RollNo==stud.RollNo)
                        {
                            students[i] = stud;
                            break;
                        }
                    }

                }

                return [200, stud,{}]
            }
        )

        $httpBackend.whenGET(new RegExp(".*")).passThrough();

    })
}());