using DbUp;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DPO
{
    class Program
    {
        public const string CONNECTION_STRING = "Server=A-305-08;Database=BusDepos;Trusted_Connection=True;";
        static void Main(string[] args)
        {

            List<Mechanic> mechanics = new List<Mechanic>
            {
                new Mechanic{
                Name="Petr"
                },
                new Mechanic{
                Name="Nikita"
                },
                new Mechanic{
                Name="Tolya"
                },
                new Mechanic{
                Name="Anton"
                },
                new Mechanic{
                Name="Michail"
                },
                new Mechanic{
                Name="Zheka"
                },

      };

            EnsureDatabase.For.SqlDatabase(CONNECTION_STRING);

            var upgrader =
              DeployChanges.To
                  .SqlDatabase(CONNECTION_STRING)
                  .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                  .LogToConsole()
                  .Build();

            upgrader.PerformUpgrade();

            MechanicRepository mechanicRepository = new MechanicRepository(CONNECTION_STRING);
            foreach (var mechanic in mechanics)
            {
                mechanicRepository.Add(mechanic);
            }

            while (true)
            {
                Console.WriteLine("Выберите меню: \n1.Посмотреть список автобусов\n2.Отремонтировать автобус\n3.Посмотреть список механиков\n4.Изменить состояние автобуса\n0.Выход");
                int choose;
                if (int.TryParse(Console.ReadLine(), out choose))
                {
                    Console.Clear();
                    switch (choose)
                    {
                        case 1:
                            {
                                BusRepository busRepository1 = new BusRepository(CONNECTION_STRING);
                                var result1 = busRepository1.GetAll();
                                Console.WriteLine($"\t Id автобуса\t|\tСостояние автобуса\t|\tId механика\t|");

                                foreach (var item in result1)
                                {
                                    Console.WriteLine($"\t{item.Id}\t|\t{item.Condition}\t|\t{item.MechanicId}\t|");
                                }
                                break;
                            }
                        case 2:
                            {
                                BusRepository busRepository2 = new BusRepository(CONNECTION_STRING);
                                Bus bus = new Bus();
                                Console.WriteLine("Добавьте автобус: ");
                                int idByBus;
                                while (true)
                                {
                                    int check = 0;
                                    var resul = busRepository2.GetAll();
                                    idByBus = new Random().Next(0);
                                    foreach (var busid in resul)
                                    {
                                        if (busid.Id == idByBus)
                                        {
                                            check++;
                                        }
                                    }
                                    if (check == 0) { break; }
                                }
                                Console.WriteLine("Выберите состояние для автобуса:\n1.Сломан\n2.Ремонтировать\n3.Отремонтирован");
                                int ch;
                                if (int.TryParse(Console.ReadLine(), out ch))
                                {
                                    switch (ch)
                                    {
                                        case 1:
                                        case 3:
                                            busRepository2.Delete(bus);
                                            break;
                                        case 2:
                                            bus.Status = 2;
                                            bus.Condition = "В ремонт";
                                            var mechs = mechanicRepository.GetAll();
                                            Mechanic mechUpdate = new Mechanic();
                                            int ind = 0;
                                            foreach (var mech in mechs)
                                            {
                                                if (mech.BusId == null) { mechUpdate = mech; ind++; }

                                            }
                                            if (ind == 0) { Console.WriteLine("Все механики заняты!"); break; }
                                            mechUpdate.BusId = bus.Id;
                                            bus.MechanicId = mechUpdate.Id;

                                            busRepository2.Add(bus);
                                            mechanicRepository.Update(mechUpdate);
                                            break;
                                    }
                                }
                            }
                            break;
                        case 3:
                            var resultMech = mechanicRepository.GetAll();
                            Console.WriteLine($"\t Id механика\t|\tИмя механика\t|\tId автобуса\t|");

                            foreach (var item in resultMech)
                            {
                                Console.WriteLine($"\t{item.Id}\t|\t{item.Name}\t|\t{item.BusId}\t|");
                            }
                            break;
                        case 4:
                            BusRepository busRepository = new BusRepository(CONNECTION_STRING);
                            var result = busRepository.GetAll();
                            Console.WriteLine($"\t Id автобуса\t|\tСостояние автобуса\t|\tId механика\t|");

                            int i = 0;
                            foreach (var item in result)
                            {
                                Console.WriteLine($"{++i}\t{item.Id}\t|\t{item.Condition}\t|\t{item.MechanicId}\t|");
                            }
                            Console.WriteLine("Выберите одного автобуса: ");
                            int busIndex;
                            if (int.TryParse(Console.ReadLine(), out busIndex))
                            {
                                if (busIndex <= result.Count)
                                {
                                    busIndex--;
                                    Bus bus = new Bus();
                                    i = 0;
                                    foreach (var busByresult in result)
                                    {
                                        if (i == busIndex)
                                        {
                                            bus = busByresult;
                                        }
                                        i++;
                                    }
                                    Console.WriteLine("Выберите состояние для автобуса:\n1.Сломан\n2.Ремонтировать\n3.Отремонтирован");
                                    int busInd;
                                    if (int.TryParse(Console.ReadLine(), out busInd))
                                    {
                                        switch (busInd)
                                        {
                                            case 1:
                                            case 3:
                                                busRepository.Delete(bus);
                                                break;
                                            case 2:
                                                break;
                                        }
                                    }
                                }
                                break;
                            }
                            break;
                        case 0:
                            return;
                    }
                }
            }
        }
    }
}
