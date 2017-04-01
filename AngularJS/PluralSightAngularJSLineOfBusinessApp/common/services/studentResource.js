// JavaScript source code

(function()
{
    "use strict";

    angular.module("common.services")
            .factory("studentResource", ["$resource", studentResource]);
     
    function studentResource($resource)
    {
        return $resource("/api/studentsRepository")
    }
}
())