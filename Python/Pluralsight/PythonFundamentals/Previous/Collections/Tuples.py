# Tuples are to hold more than one values together.

def GiveMeAStudent():
    student = ("Sumanesh", "97CS28")
    return student


# has to be unpacked into multiple variables as shown below
(name, roll) = GiveMeAStudent()

print("Name : {}, Roll : {}".format(name, roll))

# Tuples are immutable

# Number of items in the
student = GiveMeAStudent()
print("Length : {}".format(len(student)))

# Tuples can be added, multiplied
student += student

print("Added student : ", student)

student = GiveMeAStudent()
student *= 3

print("Multiplied student ", student)

# tuples can be used to swap two variables

a = 1
b = 2

(a, b) = (b, a)

print("A : {}, B : {}", a, b)

# tuples can be initialized directly using a string
strTuple = tuple("Sumanesh")

print(strTuple)

#conditional operators in tuple are in/not in

condTuple = (1, 2, 3)

print(1 in condTuple)

print(5 not in condTuple)
