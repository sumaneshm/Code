import sys
#Fake a main method as Python doesn't have any entry point method

def main(argv = None):
    if argv is None:
        print 'No arguments passed, hence retrieving from system variables'
        argv = sys.argv
    print 'Printing all the arguments'
    for oneArg in argv:
        print oneArg

if __name__ == "__main__":
    main()

    