__author__ = 'Sumanesh Magarabooshanam'
#
# def reset(b):
#     print("Before reset : {0}".format(b))
#     b[:] = [0, 1, 2, 3, 4, 5]
#     print("After reset : {0}".format(b))
#
# ############################################################################################################
#
# #type(obj) - Determines type of the object
# i = 10
# print ("i with value {0} is of type {1}".format(i,type(i)))
#
# #isinstanceof - to check whether the object supports this type
# print(" i isinstanceof int : {0}".format(isinstance(i, float)))
#
# ############################################################################################################
#
# # a real long number
# h = 123456789012345678901234567890123456789012345678901234567890
#
# print(h+5)
#
# i = 1.2e0j
# print("real : {0}, imag : {1}".format(i.real,i.imag))
#
# ############################################################################################################
#
# listOfNumbers = [1, 20, 30, 4, 5]
# print(1 in listOfNumbers)
#
# print("listOfNumbers[0] : {0} ".format(listOfNumbers[0]))
# print("listOfNumbers[-1] : {0}".format(listOfNumbers[-1]))
# print(listOfNumbers[2:4])  # Lower bound is included but upper bound is not included
#
# a = [0, 1, 2, 3, 4, 5]
# a[1:2] = [10, 20, 30]
# print(a)
# reset(a)
# print("This is a test", a)
#
# for i in range(1, 11):
#     print(i, end=" ")
#
#
# # This is called list comprehension
# a = []
# b = []
# reset(a)
# reset(b)
#
# print (a)
# print (b)
# lst = [str(x) + "_" + str(y) for x in a for y in b if (y % 2 == 0) & (x % 2 == 0)]
#
# print(lst)
#
#

def frange(start, stop, step = 1.0):
    while start < stop:
        yield start
        start += step

def upAndDown(n):
    for j in range(1, n): yield j
    for j in range(n, 0, -1): yield j

N = 6
# for i in upAndDown(N):
#     print(' ' * i, i, ' ' * (N - i) * 2, i)


for i in frange(1, 5, 0.2):
    print ("{0:.2f}".format(i), end=' ')