def convert(s):
    try:
        x = int(s)
        print("Conversion succeeded")
    except ValueError:
        print("Conversion failed due to ValueError")
        x = -1
    except TypeError:
        print("Conversion failed due to TypeError")
        x = -1
    return x


print(convert(["33s"]))
