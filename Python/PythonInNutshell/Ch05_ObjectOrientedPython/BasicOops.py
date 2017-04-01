__author__ = 'sumaneshm'

class Student:
    name = "Sumanesh"

#If we use () after the class name, this means it is creating new instance
s = Student()
a = Student()
a.name = "Aadhavan"
print (s.name, " ", a.name) #Output : Sumanesh Aadhavan

#Without (), both s1, a1 are referring to the same instance of the class and hence
s1 = Student
a1 = Student

# as they point to the same instance, if we change one name, automatically other name will also change
a1.name = "Aadhavan"

print(s1.name, " ", a1.name)                                                #Output : Aadhavan Aadhavan

print (s1.__name__, " ", s1.__bases__)                                      #Output : Student (<class 'object'>,)

# python keeps all the attributes in a hidden dictionary
print (s1.__dict__['name'])                                                 #Output : Aadhavan

# we can create new properties using the setattr or directly using the attribute name
setattr(s1, "age", 4)
s1.Mark1 = 100
print(s1.age)                                                               #Output : 4
print(s1.Mark1)

