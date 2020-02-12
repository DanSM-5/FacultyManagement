using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            //Default Faculties
            if (!context.Faculties.Any())
            {
                context.Faculties.AddRange(
                    new Faculty { Name = "Edgar Gonzalez", Email = "egonz10@centennial.ca", Phone = "6034593654" },
                    new Faculty { Name = "Carla Morales", Email = "cmor14@centennial.ca", Phone= "4567981230" },
                    new Faculty { Name = "Alberto del Rio", Email = "albert_programer@gmail.com", Phone = "1230478569" },
                    new Faculty { Name = "Javier Marquez", Phone = "4058524569", Email = "dasd@dasda.dasd" },
                    new Faculty { Name = "Beatriz Moreno", Email = "any@some.thing", Phone="1234567890" }
                    );

                context.SaveChanges();
            }

            //Default Courses
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course { Name = "Programming 2", ShortName = "Comp-123"},
                    new Course { Name = "Client-side Web Development", ShortName = "Comp-125"},
                    new Course { Name = "Web Interface Design", ShortName = "Comp-213"},
                    new Course { Name = "Java Programing", ShortName = "Comp-228"},
                    new Course { Name = "Web Application Development", ShortName = "Comp-229"}
                    );
                context.SaveChanges();
            }
        }
    }
}
