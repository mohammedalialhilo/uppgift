using WestcoastEducation.Domain.Entities;
using WestcoastEducation.Domain.Services;
using WestcoastEducation.Domain.ValueObjects;

static void Header(string title)
{
    Console.WriteLine(new string('=', 70));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 70));
}

var studentService = new StudentService();
var teacherService = new TeacherService();
var courseService  = new CourseService();

// Seed-lärare om tomt
async Task EnsureSeedAsync()
{
    if ((await teacherService.ListAsync()).Count == 0)
    {
        await teacherService.AddAsync(new Teacher{
            FirstName="Anna", LastName="Andersson", Email="anna@wce.se", Phone="070-111111",
            PersonalNumber="19800101-1234",
            ExpertiseArea="Webb/JavaScript",
            Address = new Address("Storgatan 1","41111","Göteborg")
        });
        await teacherService.AddAsync(new Teacher{
            FirstName="Björn", LastName="Berg", Email="bjorn@wce.se", Phone="070-222222",
            PersonalNumber="19750101-5678",
            ExpertiseArea="C#/.NET",
            Address = new Address("Nygatan 2","41112","Göteborg")
        });
    }
}
await EnsureSeedAsync();
while (true)
{
    Console.WriteLine();
    Header("Westcoast Education – Demo Console");
    Console.WriteLine("1) Lista lärare");
    Console.WriteLine("2) Lägg till student");
    Console.WriteLine("3) Lista studenter");
    Console.WriteLine("4) Skapa klassrumskurs");
    Console.WriteLine("5) Skapa on-demandkurs");
    Console.WriteLine("6) Lista kurser");
    Console.WriteLine("7) Koppla lärare till kurs");
    Console.WriteLine("8) Anmäl student till kurs");
    Console.WriteLine("9) Kör auto-avbokningskontroll (3 v före, min 5 deltagare)");
    Console.WriteLine("0) Avsluta");
    Console.Write("\nVälj: ");

    var key = Console.ReadLine();
    Console.WriteLine();

    try
    {
        switch (key)
        {
            case "1":
                Header("Lärare");
                foreach (var t in await teacherService.ListAsync())
                    Console.WriteLine($"{t.Id} | {t}");
                break;

            case "2":
                Header("Ny student");
                Console.Write("Förnamn: "); var fn = Console.ReadLine() ?? "";
                Console.Write("Efternamn: "); var ln = Console.ReadLine() ?? "";
                Console.Write("E-post: "); var em = Console.ReadLine() ?? "";
                Console.Write("Telefon: "); var ph = Console.ReadLine() ?? "";
                Console.Write("Personnummer: "); var pn = Console.ReadLine() ?? "";
                Console.Write("Adress: "); var street = Console.ReadLine() ?? "";
                Console.Write("Postnummer: "); var zip = Console.ReadLine() ?? "";
                Console.Write("Ort: "); var city = Console.ReadLine() ?? "";

                var s = new Student{
                    FirstName=fn, LastName=ln, Email=em, Phone=ph, PersonalNumber=pn,
                    Address = new Address(street, zip, city)
                };
                await studentService.RegisterAsync(s);
                Console.WriteLine("\n✅ Student registrerad!");
                break;

            case "3":
                Header("Studenter");
                foreach (var st in await studentService.ListAsync())
                    Console.WriteLine($"{st.Id} | {st}");
                break;

            case "4":
                Header("Ny klassrumskurs");
                Console.Write("Kursnummer: "); var no = Console.ReadLine() ?? "";
                Console.Write("Titel: "); var title = Console.ReadLine() ?? "";
                Console.Write("Längd (dagar): "); int.TryParse(Console.ReadLine(), out var days);
                Console.Write("Start (YYYY-MM-DD): "); DateTime.TryParse(Console.ReadLine(), out var start);
                Console.Write("Slut (YYYY-MM-DD): "); DateTime.TryParse(Console.ReadLine(), out var end);
                Console.Write("Plats: "); var loc = Console.ReadLine() ?? "";
                Console.Write("Är det lärarledd distans? (j/n): "); var dist = (Console.ReadLine() ?? "").ToLower()=="j";

                var c = new ClassroomCourse{
                    CourseNumber = no,
                    Title = title,
                    LengthDays = days,
                    Schedule = new Schedule(start, end, (int)Math.Ceiling((end-start).TotalDays/7.0)),
                    Location = loc,
                    IsRemoteTeacherLed = dist
                };
                await courseService.AddAsync(c);
                Console.WriteLine("\n✅ Klassrumskurs skapad!");
                break;

            case "5":
                Header("Ny on-demandkurs");
                Console.Write("Kursnummer: "); var onNo = Console.ReadLine() ?? "";
                Console.Write("Titel: "); var onTitle = Console.ReadLine() ?? "";
                Console.Write("Längd (dagar): "); int.TryParse(Console.ReadLine(), out var onDays);
                Console.Write("Start (YYYY-MM-DD): "); DateTime.TryParse(Console.ReadLine(), out var onStart);
                Console.Write("Slut (YYYY-MM-DD): "); DateTime.TryParse(Console.ReadLine(), out var onEnd);

                var oc = new OnlineCourse{
                    CourseNumber = onNo,
                    Title = onTitle,
                    LengthDays = onDays,
                    Schedule = new Schedule(onStart, onEnd, (int)Math.Ceiling((onEnd-onStart).TotalDays/7.0)),
                    PreviewChapters = new List<string>{ "Introduktion", "Smakprov: Delmoment A" }
                };
                await courseService.AddAsync(oc);
                Console.WriteLine("\n✅ On-demandkurs skapad!");
                break;

            case "6":
                Header("Kurser");
                foreach (var crs in await courseService.ListAsync())
                    Console.WriteLine($"{crs.Id} | {crs}");
                break;

            case "7":
                Header("Koppla lärare till kurs");
                Console.Write("CourseId: "); var cid = Console.ReadLine() ?? "";
                Console.Write("TeacherId: "); var tid = Console.ReadLine() ?? "";
                await courseService.AssignTeacherAsync(cid, tid);
                Console.WriteLine("\n✅ Lärare kopplad!");
                break;

            case "8":
                Header("Anmäl student till kurs");
                Console.Write("StudentId: "); var sid = Console.ReadLine() ?? "";
                Console.Write("CourseId: "); var scid = Console.ReadLine() ?? "";
                await studentService.EnrollAsync(sid, scid);
                Console.WriteLine("\n✅ Student anmäld!");
                break;

            case "9":
                Header("Auto-avbokningskontroll");
                Console.Write("CourseId: "); var acId = Console.ReadLine() ?? "";
                var canceled = await courseService.AutoCancelIfLowEnrollmentAsync(acId, 5, 3);
                Console.WriteLine(canceled
                    ? "⚠️ Kursen bör ställas in (för få anmälda)."
                    : "✅ Kursen har tillräckligt många deltagare eller startar inte inom 3 veckor.");
                break;

            case "0":
                Console.WriteLine("\nAvslutar programmet...");
                return;

            default:
                Console.WriteLine("Ogiltigt val, försök igen.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ Fel: {ex.Message}");
    }

    Console.WriteLine("\nTryck [Enter] för att återgå till menyn...");
    Console.ReadLine(); // waits until you press enter, keeps text visible
}

