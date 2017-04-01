# Dictionaries can be created like this

shares = {
    "GOOG":12.4,
    "MSFT":43.5,
    "FB":234.5,
    "APPL":345.4
}

emptyShares = {}

yetAnotherEmptyShares = dict()

for k in shares:
    print(k, shares[k])

# Check whether key exists in dictionary using "in" keyword
if "GOOG" in shares:
    print("GOOG is present")
    print("Value : %0.02f " % shares["GOOG"])