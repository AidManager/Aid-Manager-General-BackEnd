using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using CalendarEvent = AidManager.API.Collaborate.Domain.Model.Entities.Event;

using AidManager.API.ManageCosts.Domain.Model.Aggregates;
using AidManager.API.ManageCosts.Domain.Model.Commands;
using AidManager.API.ManageCosts.Domain.Model.Entities;

namespace AidManager.NUnitTests.CalendarAndAnalytics
{
    [TestFixture]
    public class CalendarAndAnalyticsStoriesTests
    {
        [Test] // HU16: Visualizar fechas vinculadas con tareas y proyectos (Event aggregate)
        public void Test_View_Tasks_And_Projects_by_Calendar()
        {
            var create = new CreateEventCommand(
                Name: "Reunión",
                Date: "2025-10-05",
                Location: "Lima",
                Description: "Detalle",
                Color: "rojo",
                ProjectId: 1
            );

            var ev = new CalendarEvent(create);

            ev.Name.Should().Be("Reunión");
            ev.Date.Should().Be("2025-10-05");
            ev.Location.Should().Be("Lima");
            ev.Description.Should().Be("Detalle");
            ev.Color.Should().Be("rojo");
            ev.ProjectId.Should().Be(1);
        }

        [Test] // HU17: Visualizar estadísticas del proyecto (Analytics aggregate)
        public void Test_Manager_View_Analytics()
        {
            var lines = new List<LineChartData>
            {
                new LineChartData(),
                new LineChartData()
            };

            var bars = new List<BarData>
            {
                new BarData(),
                new BarData()
            };

            var progress = new List<double> { 0.2, 0.4, 0.6 };
            var status   = new List<double> { 1, 0, 1 };
            var tasks    = new List<double> { 5, 7, 8 };

            var create = new CreateAnalyticsCommand(
                ProjectId: 1,
                linesChartBarData: lines,
                barData: bars,
                progressbar: progress,
                status: status,
                tasks: tasks
            );

            var an = new Analytics(create);

            an.ProjectId.Should().Be(1);
            an.LinesChartBarData.Should().BeEquivalentTo(lines);
            an.BarData.Should().BeEquivalentTo(bars);
            an.Progressbar.Should().BeEquivalentTo(progress);
            an.Status.Should().BeEquivalentTo(status);
            an.Tasks.Should().BeEquivalentTo(tasks);
        }
    }
}
