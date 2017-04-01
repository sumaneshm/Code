# This example shows how to override the properties correctly

class Student:
    def __init__(self, year, serial):
        self._year = year
        self._serial = serial

    # by defining a method which can be overridden cleanly, we eliminate the need for any hard coding
    # properties with the fully qualified name as explained in "Shipping.py"
    def _get_roll(self):
        return "{}-{}".format(self._year, self._serial)

    @property
    def roll(self):
        return self._get_roll()


class CollegeStudent(Student):
    def __init__(self, dep, year, serial):
        super().__init__(year, serial)
        self._dep = dep

    def _get_roll(self):
        return "{}{}{}".format(self._year, self._dep, self._serial)


def experiment1():
    c = CollegeStudent("CS", "97", "28")
    s = Student("95", "65")

    print("College Roll : {}, School Roll : {}".format(c.roll, s.roll))


if __name__ == '__main__':
    experiment1()
