__author__ = 'sumaneshm'

class Student:
    def InstanceMethod(self):
        print ("InstanceMethod")

s = Student()
#InstanceMethod method is bound to an object and hence called as bound method
print(s.InstanceMethod)


s.InstanceMethod()       #Calls the method
print(Student.InstanceMethod.im_class)
Student.InstanceMethod(s)   #Another way to call the instance method
