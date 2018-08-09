# itertools gives many utilities for us to workwith

# islice slices the "enumerable" in the same way as we slice a normal "list"
# count gives an "enumerable" range

from itertools import islice, count, chain

sum_of_evens_under_1000 = sum(islice((x for x in count() if x % 2 == 0), 1000))

print(sum_of_evens_under_1000)


# any and all are two useful functions which will check whether "any" or "all" the values passed as list are "True"

print(any([False, False, False]))
print(any([False, False, True]))
print(all([True, True, False]))
print(all([True, True, True]))

# it can be handy in the following case to check whether there is any multiples of 11 between
print(any(x % 11 == 0 for x in range(1, 20)))

# zip i s another one useful utility function to synchronize two interables

sunday = list(range(1, 10))
monday = list(range(20, 30))
tuesday = list (range(30, 40))

# zip yields tuples of values from multiple enumerables, so we can directly unpack them
for sun_temp, mon_temp in zip(sunday, monday):
    print("Average : ", (sun_temp + mon_temp) / 2)

# zip accepts any number of parameters
# yet another powerful feature of zip, (notice are not unzipping the tuple in the fore loop and how it is useful)d
for temps in zip(sunday, monday, tuesday):
    print("Min : {:4.1f}, Max : {:4.1f}, Average : {:4.1f}".format(min(temps), max(temps), sum(temps)/len(temps)))

# chain utility method chains one more enumerables

all_temperatures = chain(sunday, monday, tuesday)

# using this we can check whether all the days had positive temperatures
print(all(t > 0 for t in all_temperatures))
