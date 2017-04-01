(function ()
{
    "use strict";

    angular.module("common.services")
        .factory("studentService",studentService);

    function studentService()
    {
        function calculateFee(student)
        {
            if(student && student.Age)
            {
                if(student.Age>18)
                {
                    return student.Age * 1000;
                }
                else
                {
                    return student.Age * 100;
                }
            }
        }

        function getSalutation(student)
        {
            if(student && student.Age)
            {
                if(student.Age>18)
                {
                    return "Mr./Mdm.";
                }
                else
                {
                    return "Master/Miss";
                }
            }
        }

        function getAverage(student)
        {
            if(student && student.Marks)
            {
                var sum = getTotalMarks(student);
                return sum / student.Marks.length;

            }
        }

        function getTotalMarks(student)
        {
            if(student && student.Marks)
            {
                var sum = 0;
                for(var i=0;i<student.Marks.length;i++)
                {
                    sum += parseInt(student.Marks[i]);
                }

                return sum;
            }
        }

        function getResult(student)
        {
            if(student && student.Marks)
            {
                for(var i=0;i<student.Marks.length;i++) {
                    if(isNaN(student.Marks[i]))
                        return "N/A";

                    if (student.Marks[i] < 40)
                    {
                        return "Fail";
                    }
                }

                return "Pass";
            }

            return "N/A";
        }

        return {
            getSalutation : getSalutation,
            calculateFee : calculateFee,
            getAverage : getAverage,
            getTotalMarks : getTotalMarks,
            getResult: getResult
        };
    }
}());