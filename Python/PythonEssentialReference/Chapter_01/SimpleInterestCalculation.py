__author__ = 'sumaneshm'

principal = 1000        # This is a comment
rate = 0.05
numYears = 5
year = 1

while year <= numYears:
    principal *= 1 + rate
    print("%3d %0.02f" % (year, principal))
    year += 1

