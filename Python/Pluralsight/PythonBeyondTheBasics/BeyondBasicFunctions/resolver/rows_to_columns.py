from pprint import pprint as pp

def experiment1():
    student1 = (21, 22, 23, 24, 25)
    student2 = (25, 24, 23, 22, 21)
    student3 = (23, 25, 25, 21, 22)

    students = [student1, student2, student3]

    # zip *variable transposes rows to columns
    pp(list(zip(*students)))

if __name__ == '__main__':
    experiment1()