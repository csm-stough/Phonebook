using csm_stough.Phonebook;
using csm_stough.Phonebook.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spectre.Console;
using System.Text.RegularExpressions;

public class Driver
{
    static PhonebookContext context { get; set; }

    public static void Main(string[] args)
    {
        context = new PhonebookContext();

        ContactsMenu();
    }

    public static void ContactsMenu()
    {
        AnsiConsole.Clear();

        List<Contact> Contacts = context.Contacts.OrderBy(c => c.Name).ToList();

        SelectionPrompt<MenuOption> selectionPrompt = new SelectionPrompt<MenuOption>()
            .Title("Contacts")
            .AddChoices(
                Contacts.Select(contact => new MenuOption() { text = contact.Name, action = () => { ContactMenu(contact); } })
            )
            .UseConverter(selection => selection.text);

        selectionPrompt.AddChoice(new MenuOption() { text = "New Contact...", action = () => { NewContactMenu(); } });
        selectionPrompt.AddChoice(new MenuOption() { text = "Exit Application", action = () => { Environment.Exit(0); } });

        MenuOption selection = AnsiConsole.Prompt(selectionPrompt);

        selection.action();
    }

    public static void ContactMenu(Contact contact)
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine(contact.Name);
        AnsiConsole.WriteLine(contact.PhoneNumber);
    }

    public static void NewContactMenu()
    {
        AnsiConsole.Clear();

        Regex phoneNumberRegex = new Regex(System.Configuration.ConfigurationManager.AppSettings.Get("PhoneNumberRegex"));
        string Name = AnsiConsole.Ask<string>("Please enter the contact name: ");

        while (context.Contacts.Where(c => c.Name.Equals(Name)).Count() > 0)
        {
            AnsiConsole.WriteLine($"Name {Name} already exists. Please enter another...");
            Name = AnsiConsole.Ask<string>("Please enter the contact name: ");
        }

        string PhoneNumber = AnsiConsole.Ask<string>($"Please enter the phone number for {Name}:");

        while(!phoneNumberRegex.Match(PhoneNumber).Success)
        {
            AnsiConsole.WriteLine("Improper phone number format. Please try again");
            PhoneNumber = AnsiConsole.Ask<string>($"Please enter the phone number for {Name}:");
        }

        context.Contacts.Add(new Contact() { Name = Name, PhoneNumber = PhoneNumber });
        context.SaveChanges();

        AnsiConsole.WriteLine("New Contact Added!");

        ContactsMenu();
    }
}
