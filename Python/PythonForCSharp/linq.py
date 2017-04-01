from AnonymousObject import AnonObject

__author__ = 'sumaneshm'


class Person:
    def __init__(self, name, age):
        self.age = age
        self.name = name

    def __str__(self):
        return "{0} is {1} years old".format(self.name, self.age)

    def __repr__(self):
        return self.__str__()

people = [
    Person("Sumanesh", 34),
    Person("Saveetha",31),
    Person("Aadhavan",4),
    Person("Aghilan",0.3)
]

query = [
    AnonObject(name=p.name,age=p.age)
    for p in people
    if p.age < 20
]

query.sort(key = lambda p : p.name)

for p in query:
    print (type(p), p)
