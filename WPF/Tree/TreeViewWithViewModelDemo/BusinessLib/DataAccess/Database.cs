namespace BusinessLib
{
    /// <summary>
    /// A data source that provides raw data objects.  In a real
    /// application this class would make calls to a database.
    /// </summary>
    public static class Database
    {
        #region GetRegions

        public static Region[] GetRegions()
        {
            return new Region[]
            {
                new Region("Northeast"),
                new Region("Midwest")
            };
        }

        #endregion // GetRegions

        #region GetStates

        public static State[] GetStates(Region region)
        {
            switch (region.RegionName)
            {
                case "Northeast":
                    return new State[]
                    {
                        new State("Connecticut"),
                        new State("New York")
                    };

                case "Midwest":
                    return new State[]
                    {
                        new State("Indiana")
                    };
            }

            return null;
        }

        #endregion // GetStates

        #region GetCities

        public static City[] GetCities(State state)
        {
            switch (state.StateName)
            {
                case "Connecticut":
                    return new City[]
                    {
                        new City("Bridgeport"),
                        new City("Hartford"),
                        new City("New Haven")
                    };

                case "New York":
                    return new City[]
                    {
                        new City("Buffalo"),
                        new City("New York"),
                        new City("Syracuse")          
                    };

                case "Indiana":
                    return new City[]
                    {
                        new City("Evansville"),
                        new City("Fort Wayne"),
                        new City("Indianapolis"),
                        new City("South Bend")
                    };
            }

            return null;
        }

        #endregion // GetCities

        #region GetFamilyTree

        public static Person GetFamilyTree()
        {
            // In a real app this method would access a database.
            return new Person
            {
                Name = "David Weatherbeam",
                Age = 85,
                RollNumber = "Roll1",
                ExaminationNumber = "Exam1",
                Children =
                {
                    //new Person
                    //{
                    //    Name="Alberto Weatherbeam",
                    //    Age=65,
                    //    Children=
                    //    {
                    //        new Person
                    //        {
                    //            Name="Zena Hairmonger",
                    //            Age=35,
                    //            Children=
                    //            {
                    //                new Person
                    //                {
                    //                    Age=5,
                    //                    Name="Sarah Applifunk",
                    //                }
                    //            }
                    //        },
                    //        new Person
                    //        {
                    //            Name="Jenny van Machoqueen",
                    //            Age=34,
                    //            Children=
                    //            {
                    //                new Person
                    //                {
                    //                    Age=3,
                    //                    Name="Nick van Machoqueen",
                    //                },
                    //                new Person
                    //                {
                    //                     Age=3,
                    //                    Name="Matilda Porcupinicus",
                    //                },
                    //                new Person
                    //                {
                    //                     Age=3,
                    //                    Name="Bronco van Machoqueen",
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    //new Person
                    //{
                    //    Name="Komrade Winkleford",
                    //     Age=83,
                    //    Children=
                    //    {
                    //        new Person
                    //        {
                    //            Name="Maurice Winkleford",
                    //             Age=28,
                    //            Children=
                    //            {
                    //                new Person
                    //                {
                    //                     Age=3,
                    //                    Name="Divinity W. Llamafoot",
                    //                }
                    //            }
                    //        },
                    //        new Person
                    //        {
                    //            Name="Komrade Winkleford, Jr.",
                    //             Age=43,
                    //            Children=
                    //            {
                    //                new Person
                    //                {
                    //                     Age=13,
                    //                    Name="Saratoga Z. Crankentoe",
                    //                },
                    //                new Person
                    //                {
                    //                     Age=13,
                    //                    Name="Excaliber Winkleford",
                    //                }
                    //            }
                    //        }
                    //    }
                    //},
                    new Person
                        {
                            Name = "Sumanesh",
                             Age=35,
                             RollNumber = "97CS28",
                             ExaminationNumber = "12345",
                            Children=
                                {
                                    new Person
                                        {
                                            Age=1,
                                            RollNumber = "Roll 2",
                                            ExaminationNumber = "Exam 2",
                                            Name="Aadhavan"
                                        },
                                        new Person
                                            {
                                                 Age=2,
                                            RollNumber = "Roll 3",
                                            ExaminationNumber = "Exam 3",

                                                Name = "Nila"
                                            }
                                }
                        }
                }
            };
        }

        #endregion // GetFamilyTree
    }
}