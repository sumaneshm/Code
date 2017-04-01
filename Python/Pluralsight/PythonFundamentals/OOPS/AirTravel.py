# RnD -> How is python initializer different from other constructors
# Rnd -> What is law of demeter (only talk to your friends law) design principal

from pprint import pprint as pp

class Flight:
    # this is just an initializer and not a constructor (the object is already
    # created and we are just trying to initialize the properties here

    def __init__(self, number, aircraft):  # initializer
        if not number[:2].isalpha():
            raise ValueError("First two characters should be airline code")
        if not number[:2].isupper():
            raise ValueError("Airline code 98.8."
                             "3680should be given in uppercase")
        if (not number[2:].isdigit()) or (not int(number[2:]) <= 9999):
            raise ValueError("Flight number should be integer <= 9999")
        self._number = number
        self._aircraft = aircraft

        rows, seats = self.aircraft_seating_plan()
        self._seating = [None] + [{letter:None for letter in seats} for _ in rows]

    def number(self):
        return self._number

    def airline(self):
        return self._number[:2]

    def aircraft_total_seats(self):
        return self._aircraft.total_seats()

    def aircraft_model(self):
        return self._aircraft.model()

    def aircraft_seating_plan(self):
        return self._aircraft.seating_plan()

    # method names start with _ "indicate" that it is a private method (just convention, still visible to the callers)
    def _parse_seat(self, seat):
        rows, seats = self.aircraft_seating_plan()

        letter = seat[-1]

        if letter not in seats:
            raise ValueError("Invalid Seat letter. It should be within {}".format(seats))

        string_row = seat[:-1]
        if not string_row.isdigit():
            raise ValueError("Row should be a number")

        row = int(string_row)
        if row not in rows:
            raise ValueError("Row number {} should be within {}".format(row, list(rows)))

        return row, letter

    def allocate_seat(self, seat, passenger_name):
        # example for Seat : 1A, 10F
        row, letter = self._parse_seat(seat)

        if self._seating[row][letter] is not None:
            raise ValueError("Seat {} is already allocated to somebody".format(seat))

        self._seating[row][letter] = passenger_name
        print("Seat {} has been successfully allocated to {}".format(seat, passenger_name))

    def reallocate_seat(self, current_seat, new_seat):
        current_row, current_letter = self._parse_seat(current_seat)
        if self._seating[current_row][current_letter] is None:
            raise ValueError("Current seat {} is empty".format(current_seat))

        new_row, new_letter = self._parse_seat(new_seat)
        if self._seating[new_row][new_letter] is not None:
            raise ValueError("New seat {} is already occupied".format(new_seat))

        self._seating[new_row][new_letter]  = self._seating[current_row][current_letter]
        self._seating[current_row][current_letter] = None

    # yet another python beauty
    def num_available_seats(self):
        return sum(sum(1 for _ in row.values() if _ is None) for row in self._seating if row is not None)

    def _list_all_passengers(self):
        rows, letters = self.aircraft_seating_plan()
        for row in rows:
            for letter in letters:
                passenger = self._seating[row][letter]
                if passenger is not None:
                    yield passenger, "{}{}".format(row, letter)

    # dependency injection in python. Inject the method on which it has to be performed/
    # currently we are passing a console_card_printer and it can be easily replaced
    # by a real card printer easily without impacting the Flight

    def print_boarding_cards(self, card_printer):
        for passenger, seat in self._list_all_passengers():
            card_printer(passenger, seat, self._number, self.aircraft_model())


# Aircraft is a base class created to be an abstract (we are accessing self.seating_plan which doesn't exist yet)
class Aircraft:
    # def __init__(self, registration, model, num_rows, num_seats_per_row):
    #     self._model = model
    #     self._registration = registration
    #     self._num_rows = num_rows
    #     self._num_seats_per_row = num_seats_per_row
    #
    # def registration(self):
    #     return self._registration
    #
    # def model(self):
    #     return self._model
    #
    # def seating_plan(self):
    #     return (range(1, self._num_rows + 1), "ABCDEFGHIJ"[:self._num_seats_per_row])
    def __init__(self, registration):
        self._registration = registration

    def registration(self):
        return self._registration

    def total_seats(self):
        rows, letters = self.seating_plan()
        return len(rows) * len(letters)


# We can inherit in python by declaring base class inside paranthesis after the derived class
class AirbusA319(Aircraft):

    def model(self):
        return "Airbus A319"

    def seating_plan(self):
        return range(1, 5), "ABCDE"


class Boeing777(Aircraft):

    def model(self):
        return "Boeing A319"

    def seating_plan(self):
        return range(1, 11), "ABCDEFG"

def make_flight():
    f = Flight("SQ123", AirbusA319("B-1234"))
    f.allocate_seat("1A", "Sumanesh")
    f.allocate_seat("1B", "Saveetha")
    f.allocate_seat("2A","Aadhavan")
    f.allocate_seat("2B", "Aghilan")
    return f

def console_card_printer(passenger, seat, flight_number, aircraft):
    output = "| Name : {}, "         \
                "Seat : {}, "        \
                "Flight : {}, "      \
                "Aircraft : {} |".format(passenger, seat, flight_number, aircraft)
    banner = "+" + "-" * (len(output) - 2) + "+"
    border = "|" + " " * (len(output) - 2) + "|"

    lines = [banner, border, output, border, banner]

    card = "\n".join(lines)
    print(card)
    print()



def main():
    # --- Experiment : 1, How to declare a simple class & object ------------------------
    # f = Flight("SQ123", Aircraft("A360", "Airbus 360", num_rows=22, num_seats_per_row=5))

    #f.allocate_seat("10A","Sumanesh")
    #f.allocate_seat("10A","Saveetha")

    # note that we have not called the function number by passing f to the argument
    # self because "f.number()" is the syntactic sugarcoat for "Flight.number(f)"

    #print(Flight.number(f))  # Thank goodness for the sugarcoat

    #print(f.airline())

    # --- Experiment : 2, A deep dive into classes and beauty of Python ------------------------
    # f = make_flight()
    # print("Current seat allocation....")
    # pp(f._seating)
    # f.reallocate_seat("2A", "1C")
    # f.reallocate_seat("2B", "1D")
    # print("After reallocation...")
    # pp(f._seating)
    # print("Total available seats : {}".format(f.num_available_seats()))

    # --- Experiment : 3 ------------------------
    # console_card_printer("Sumanesh", "10G", "SQ123", "A380")
    f = make_flight()
    # f.print_boarding_cards(console_card_printer)
    print("Total number of seats in the Aircraft : {}".format(f.aircraft_total_seats()))
    print("Total available seats : {} ".format(f.num_available_seats()))


if __name__ == "__main__":
    main()
