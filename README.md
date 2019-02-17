# CSC348/M48 Web Application Development Assessment
### Introduction
The aim of the assessment is primarily to ensure you have met the learning outcomes which can be found in the module description. In addition, the coursework has been designed to achieve the following objectives:<br/>
* To provide evidence that you can work effectively with ASP.NET.
* To encourage you to independently research the concepts relating to web application development.
* To enable you to produce a web application you could use in a portfolio when applying for jobs.
* To give you the opportunity to stretch yourself and produce a high quality web application to be proud of.
<br/>
You will be given a project brief to design and implement a small web application. There will then be two pieces of coursework: a code review and the application itself. These components will all be submitted via blackboard. If there is any indication of academic misconduct it will be reported as detailed in the departmental handbook. Handing in your coursework after the deadline will result in a mark of zero.<br/>

### Project Brief
Your task is to plan and create a simple web application, using ASP.NET Core MVC6, which has the following functionalities:<br/>

1. (CSC348 and CSCM48) The web application admin, who are your clients, can create posts and their users, who are your client’s customers, can comment on those posts.
2. (CSCM48 only) The web application admin can see basic analytics information to see which of their customers are engaging with their posts.

For example, you could create a simple blog where the admin can post articles which users can comment on, or a simple version of blackboard where admin (or lecturers) can post lecture slides and students can comment on them.<br/>
You will be required to hand in two pieces of coursework throughout the semester. This will require good time management to balance it with other modules, if this is something you struggle with watch the video I made about how I manage my own time.

||| Weighting | Deadline
--- | --- | --- | ---
**Coursework 1** | Code Review | 40% | 12th November 2018 for your own code<br/>19th November 2018 to review someone else’s code
**Coursework 2** | Implementation | 60% | 8th January 2019

###  Coursework 1 – Code Review (40% weighting)
An important part of working in a software engineering organisation is code reviews. In this process engineers look at each other’s code to spot bugs and ensure standards are being adhered to. The concept of code reviews is discussed in Lecture 5.

* You must first submit one C# file from your project to be reviewed by another member of the class. This should be submitted as a pdf which includes syntax colouring.
* Ensure the C# file you select contains a significant proportion of code not automatically created by Visual Studio.
* You will then be assigned to review and mark another student’s code (within one week)
* I will then mark your review and code myself which will be used for this coursework component.

### Coursework 2 – Implementation (60% weighting)
This is the implementation of your web application using MVC 6 targeting ASP.NET Core 2.0 with the code first convention. This should be submitted as a .zip file containing the Visual Studio 2017 solution, the .zip file is required to be titled as your student number – failing to do this will be considered not meeting basic requirements. Ensure that your project works on multiple machines. **If your application does not meet these basic requirements it will be considered not working code.** Submitting code which works on multiple machines shows you understand the framework you are using and following guidelines to the letter is an essential skill for a web developer and software engineer.<br/>
<br/>
**You are also required to submit a 60 second video showing that your web application runs and meets the basic requirements.**<br/>
<br/>
It is essential that:

* You seed the following users in your authentication database
  * Member1@email.com
  * Customer1, Customer2, Customer3, Customer4, Customer5 (@email.com)
  * These users should all have the same password – “Password123!”
  
This coursework is partially self-assessed. The coursework is submitted as the first question of a blackboard test. There are then a series of questions for you to answer about your coursework. This is to show that you have understood the theory behind your implementation.<br/>
<br/>
You should read all these questions before you start working and design your implementation to clearly show me the skills. I will only give marks for questions you have answered yourself, I will check that you are correct before giving the mark.<br/>
<br/>
I recommend making a word document containing the questions, answering them there and copy and pasting them when you submit.

### Getting Support
At any time if you are feeling worried or stuck with your project ask for support. This can be done in several ways:
* I will leave time at the end of each lecture for you to ask questions
* Attend the timetabled lab classes
* Ask each other for help on slack or in real life
* Send me an e-mail (I work 9-5 Mon-Fri and will be taking annual leave around Christmas)
* Come to my office hour (this is a specific time I will be in my office; you can come outside of this time, but I may not be in my office)
* Engage with me, suggest topics I should go over in more detail in lectures or could make a video tutorial about. If it is possible I’ll do it.
* Google it
<br/>
Asking for support will in no way effect your final mark.

### Some Tips
* Plan ahead and don’t leave things to the last minute. There is no exam, all the time you would normally spend revising should be sent working on this assignment.
* Use the rubrics to mark your own (and each other’s) draft work
* If you are unsure if you have done enough look at the rubric.
* Try not to think about this just as coursework where you are trying to get a good mark. Try to enjoy creating something.

### CW1 Rubric
|| Clear Fail (<35%) Fail (35-39%) | Pass (40-49%) | 2:2 (50-59%) | 2:1 (60-69%) | First (70%-79%) | Distinction (80%+)
--- | --- | --- | --- | --- | --- | ---
Your Implementation (50%) | No code has been submitted | Source code has been submitted in the way described in the assignment. | The student has independently implemented a significant portion of the source file. | The student has followed good practices both in terms of the MVC pattern and the coding conventions for the languages used. | Each method is easy to follow and has a single responsibility | Implementation of a professional standard
Code Review (50%) | No code review has been submitted, or the language and tone is aggressive and/or personal. | Areas of improvement in the source code are identified. | Positive aspects of the source code are identified. | Solutions to bugs or ways of improving the code are explained fully. | The mark awarded is within one grade boundry of the lecturers. | A professional code review and the mark given matches the grade boundry of the lecturers.

### CW2 Rubric
<table>
  <tr>
    <th></th>
    <th>Clear Fail (<35%)</th>
    <th>Fail (35-39%)</th> 
    <th>Pass (40-49%)</th>
    <th>2:2 (50-59%)</th>
    <th>2:1 (60-69%)</th>
    <th>First (70%-79%)</th>
    <th>Distinction (80%+)</th>
  </tr>
  <tr>
    <td>Progress (20%)</td>
    <td colspan="2">Minimal progress, student has no working code</td> 
    <td>Student has working code to present but it has not progressed much beyond an introductory MVC tutorial (basic CRUD)<br/>[10]</td>
   <td>Student has implemented a web application but there are a number of bugs which would make the application unsuitable for release<br/>[2]</td>
   <td>There are some minor bugs but the application is in a usable state.<br/>[2]</td>
   <td>There is no evidence of bugs<br/>[3]</td>
   <td>A marketable web application has been produced<br/>[3]</td>
  </tr>
  <tr>
    <td>Implementation (20%)</td>
    <td colspan="2">Minimal progress, student has no working code</td> 
    <td>There is working code but the vast majority has been generated using MVC scaffolding, the student has only independently implemented a model definition.<br/>[10]</td>
   <td>Evidence that the student has independently implemented a significant portion of the application.<br/>[2]</td>
   <td>The student has followed good practices (as described in lectures) both in terms of the MVC pattern and the coding conventions for the languages used.<br/>[2]</td>
   <td>The student has used version control<br/>[3]</td>
   <td>Implementation of a professional standard<br/>[3]</td>
  </tr>
 <tr>
    <td>Security (20%)</td>
    <td colspan="2">The web application contains one or more examples of a serious security flaw</td> 
    <td>There is no obvious cross site request forgery vulnerability<br/>[10]</td>
   <td>There is no obvious cross site scripting vulnerability or over posting risk<br/>[2]</td>
   <td>Steps have been taken to prevent cookie theft<br/>[2]</td>
   <td>Steps have been taken to mitigate the risks posed by error reporting<br/>[3]</td>
   <td>An example of a security risk not mentioned in this mark scheme is mitigated.<br/>[3]</td>
  </tr>
 <tr>
    <td>Working with Models (10%)</td>
    <td colspan="2">No database is used</td> 
    <td>A database is created from a model definition using the code first convention.<br/>[5]</td>
   <td>Data annotations are used effectively.<br/>[1]</td>
   <td>At least one view specific model is used correctly<br/>[1]</td>
   <td>The database is seeded using C# code when the application starts.<br/>[1]</td>
   <td>A repository pattern is used to abstract away the data access layer and dependency injection is used to access the database.<br/>[2]</td>
  </tr>
 <tr>
    <td>Identity (10%)</td>
    <td colspan="2">User accounts are not used in any way in application</td> 
    <td>Users can log in to the web application but it serves no purpose.<br/>[5]</td>
   <td>Different users have different levels of authorisation<br/>[1]</td>
   <td>A claim based authorisation system is implemented using role names.<br/>[1]</td>
   <td>claim based authorisation system is implemented using the claims system in ASP.NET Identity.<br/>[1]</td>
   <td>An interface for site owners to create and edit users and user roles is implemented<br/>[2]</td>
  </tr>
 <tr>
    <td>Functionality (10%)</td>
    <td colspan="2">The application is a non-interactive web site</td> 
    <td>The application only has the functionality stated in the basic requirements<br/>[5]</td>
   <td>The web application has at least one additional working feature<br/>[1]</td>
   <td>The web application uses JavaScript to improve the user experience<br/>[1]</td>
   <td colspan="2">Evidence of independent research (A feature is implemented which requires knowledge from outside the taught content)<br/>[3]</td>
  </tr>
 <tr>
    <td>Presentation (10%)</td>
    <td colspan="3">The web application is unchanged from the original template<br/>[5]</td>
   <td>The web application visual style has changed from the original template<br/>[1]</td>
   <td>All styling is done using CSS<br/>[1]</td>
   <td>The web application layout changes appropriately as the browser window is resized<br/>[1]</td>
   <td>The web application is unrecognisable as a ASP.NET project<br/>[2]</td>
  </tr>
</table>
