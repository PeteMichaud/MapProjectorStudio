using System;
using NUnit.Framework;
using MapProjectorLib;

namespace MapProjectorCLI.Tests
{
    [TestFixture]
    public class CLIParamsTest : TestsWithParser
    {
        const double OneDay = 2 * Math.PI / 365.25;
        const double OneDegree = 2 * Math.PI / 360.0;
        const double OneHour = 2 * Math.PI / 24.0;

        static double ToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }
                
        [Test]
        public void SanityTest()
        {
            var args = ToArgs("");
            Assert.DoesNotThrow(() => CLIParams.Parse(args));
        }

        [Test]
        public void SetProjectionTest()
        {
            var args = ToArgs("");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(MapProjection.LatLong, cliParams.TargetProjection);
            });

            foreach(MapProjection proj in Enum.GetValues(typeof(MapProjection)))
            {
                args = ToArgs($"--projection {proj}");
                Parse(args, cliParams =>
                {
                    Assert.AreEqual(proj, cliParams.TargetProjection);
                });
            }
        }

        [Test]
        public void SetQualityTest()
        {
            var args = ToArgs("");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(ColorSampleMode.Fast, cliParams.ColorSampleMode);
            });

            foreach (ColorSampleMode proj in Enum.GetValues(typeof(ColorSampleMode)))
            {
                args = ToArgs($"--quality {proj}");
                Parse(args, cliParams =>
                {
                    Assert.AreEqual(proj, cliParams.ColorSampleMode);
                });
            }
        }

        [Test]
        public void SetInputFile()
        {
            var val = "c:\\path\\to\\file.png";
            var args = ToArgs($"-f {val} -o out.png", withDefaults: false);
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.srcImageFileName);
            });

            args = ToArgs($"--file {val} -o out.png", withDefaults: false);
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.srcImageFileName);
            });


        }

        [Test]
        public void SetOutputFile()
        {
            var val = "c:\\path\\to\\file.png";
            var args = ToArgs($"-o {val} -f file.png", withDefaults: false);
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.outImageFileName);
            });

            args = ToArgs($"--out {val} -f file.png", withDefaults: false);
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.outImageFileName);
            });
        }

        [Test]
        public void SetAdjust()
        {
            var args = ToArgs($"--adjust");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(true, cliParams.Adjust);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(false, cliParams.Adjust);
            });
        }

        [Test]
        public void SetWidth()
        {
            var val = 1000;
            var args = ToArgs($"-w {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.Width);
            });

            args = ToArgs($"--width {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.Width);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.Width);
            });
        }
        
        [Test]
        public void SetHeight()
        {
            var val = 1000;
            var args = ToArgs($"-h {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.Height);
            });

            args = ToArgs($"--height {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.Height);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.Height);
            });
        }

        [Test]
        public void SetBgColor()
        {
            var val = "255,255,255";
            var args = ToArgs($"--bgcolor {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams._backgroundColorValues);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(null, cliParams._backgroundColorValues);
            });
        }

        [Test]
        public void SetInvert()
        {
            var args = ToArgs($"-i");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(true, cliParams.Invert);
            });

            args = ToArgs($"--invert");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(true, cliParams.Invert);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(false, cliParams.Invert);
            });
        }

        [Test]
        public void SetLoopCount()
        {
            var val = 10;
            var args = ToArgs($"--loop {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.LoopCount);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1, cliParams.LoopCount);
            });
        }

        [Test]
        public void EnsureLegalLoopCount()
        {
            var val = -2;
            var args = ToArgs($"--loop {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1, cliParams.LoopCount);
            });

        }

        [Test]
        public void SetTiltIncr()
        {
            var val = 1;
            var args = ToArgs($"--tiltinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.TiltIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.TiltIncr);
            });
        }

        [Test]
        public void SetTurnIncr()
        {
            var val = 1;
            var args = ToArgs($"--turninc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.TurnIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.TurnIncr);
            });
        }

        [Test]
        public void SetLatIncr()
        {
            var val = 1;
            var args = ToArgs($"--latinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.LatIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.LatIncr);
            });
        }

        [Test]
        public void SetLongIncr()
        {
            var val = 1;
            var args = ToArgs($"--longinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.LongIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.LongIncr);
            });
        }

        [Test]
        public void SetXIncr()
        {
            var val = 1;
            var args = ToArgs($"--xinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.xIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.xIncr);
            });
        }

        [Test]
        public void SetYIncr()
        {
            var val = 1;
            var args = ToArgs($"--yinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.yIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.yIncr);
            });
        }

        [Test]
        public void SetZIncr()
        {
            var val = 1;
            var args = ToArgs($"--zinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.zIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.zIncr);
            });
        }

        [Test]
        public void SetDateIncr()
        {
            var val = 1;
            var args = ToArgs($"--dateinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDay, cliParams.DateIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.DateIncr);
            });
        }

        [Test]
        public void SetTimeIncr()
        {
            var val = 1;
            var args = ToArgs($"--timeinc {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneHour, cliParams.TimeIncr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.TimeIncr);
            });
        }

        [Test]
        public void SetTilt()
        {
            var val = 1;
            var args = ToArgs($"--tilt {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.tilt);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.tilt);
            });
        }

        [Test]
        public void SetTurn()
        {
            var val = 1;
            var args = ToArgs($"--turn {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.turn);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.turn);
            });
        }

        [Test]
        public void SetRotate()
        {
            var val = 1;
            var args = ToArgs($"--rotate {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.rotate);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.rotate);
            });
        }

        [Test]
        public void SetLat()
        {
            var val = 1;
            var args = ToArgs($"--lat {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.lat);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.lat);
            });
        }

        [Test]
        public void SetLon()
        {
            var val = 1;
            var args = ToArgs($"--lon {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.lon);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.lon);
            });
        }

        [Test]
        public void SetScale()
        {
            var val = 10;
            var args = ToArgs($"--scale {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.scale);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.0, cliParams.scale);
            });
        }

        [Test]
        public void SetRadius()
        {
            var val = 10;
            var args = ToArgs($"--radius {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val * OneDegree, cliParams.radius);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.radius);
            });
        }

        [Test]
        public void SetXOffset()
        {
            var val = 10;
            var args = ToArgs($"--xoff {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.xOffset);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.xOffset);
            });
        }

        [Test]
        public void SetYOffset()
        {
            var val = 10;
            var args = ToArgs($"--yoff {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.yOffset);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.yOffset);
            });
        }

        [Test]
        public void SetA()
        {
            var val = 10;
            var args = ToArgs($"-a {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.a);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.0, cliParams.a);
            });
        }
        //20 * Math.PI / 180.0

        [Test]
        public void SetAW()
        {
            var val = 10;
            var args = ToArgs($"--aw {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(ToRadians(val), cliParams.aw);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(ToRadians(20), cliParams.aw);
            });
        }

        [Test]
        public void SetX()
        {
            var val = 10;
            var args = ToArgs($"-x {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.x);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(8.0, cliParams.x);
            });
        }

        [Test]
        public void SetY()
        {
            var val = 10;
            var args = ToArgs($"-y {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.y);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0.0, cliParams.y);
            });
        }

        [Test]
        public void SetZ()
        {
            var val = 10;
            var args = ToArgs($"-z {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.z);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0.0, cliParams.z);
            });
        }

        [Test]
        public void SetOX()
        {
            var val = 10;
            var args = ToArgs($"--ox {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.ox);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.0, cliParams.ox);
            });
        }

        [Test]
        public void SetOY()
        {
            var val = 10;
            var args = ToArgs($"--oy {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.oy);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.0, cliParams.oy);
            });
        }

        [Test]
        public void SetOZ()
        {
            var val = 10;
            var args = ToArgs($"--oz {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.oz);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.1, cliParams.oz);
            });
        }

        [Test]
        public void SetSun()
        {
            var args = ToArgs($"--sun");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(true, cliParams.sun);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(false, cliParams.sun);
            });
        }

        [Test]
        public void SetTime()
        {
            var val = 10;
            var args = ToArgs($"--time {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.time);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.time);
            });
        }

        [Test]
        public void SetDate()
        {
            var val = 10;
            var args = ToArgs($"--date {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.date);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.date);
            });
        }


        [Test]
        public void SetP()
        {
            var val = 10;
            var args = ToArgs($"-p {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.p);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.p);
            });
        }

        [Test]
        public void SetConic()
        {
            var val = 10;
            var args = ToArgs($"--conic {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.conic);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(1.0, cliParams.conic);
            });
        }

        [Test]
        public void SetConicR()
        {
            var val = 10;
            var args = ToArgs($"--conicr {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.conicr);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.conicr);
            });
        }

        [Test]
        public void SetWidget()
        {
            //Defaults to None
            var args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(MapWidget.None, cliParams.Widgets);
            });
            
            //All Widgets are assignable
            foreach(MapWidget w in Enum.GetValues(typeof(MapWidget)))
            {
                if (w == MapWidget.None) continue;

                args = ToArgs($"--widget {w}");
                Parse(args, cliParams =>
                {
                    Assert.True(cliParams.Widgets.HasFlag(w));
                });
            }

            //Combined widget flags work
            args = ToArgs($"--widget altitudes,tropics,temporaryhours");
            Parse(args, cliParams =>
            {
                Assert.True(cliParams.Widgets.HasFlag(MapWidget.Altitudes));
                Assert.True(cliParams.Widgets.HasFlag(MapWidget.Tropics));
                Assert.True(cliParams.Widgets.HasFlag(MapWidget.TemporaryHours));
                Assert.False(cliParams.Widgets.HasFlag(MapWidget.Analemma));
            });
        }

        [Test]
        public void SetGridX()
        {
            var val = 10;
            var args = ToArgs($"--gridx {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.gridx);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(30, cliParams.gridx);
            });
        }

        [Test]
        public void SetGridY()
        {
            var val = 10;
            var args = ToArgs($"--gridy {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.gridy);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(30, cliParams.gridy);
            });
        }

        [Test]
        public void SetGridColor()
        {
            var val = "255,255,255";
            var args = ToArgs($"--gridcolor {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams._gridColorValues);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(null, cliParams._gridColorValues);
            });
        }

        [Test]
        public void SetWidgetColor()
        {
            var val = "255,255,255";
            var args = ToArgs($"--widgetcolor {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams._widgetColorValues);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(null, cliParams._widgetColorValues);
            });
        }

        [Test]
        public void SetWLat()
        {
            var val = 10;
            var args = ToArgs($"--wlat {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(ToRadians(val), cliParams.widgetLat);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.widgetLat);
            });
        }

        [Test]
        public void SetWLon()
        {
            var val = 10;
            var args = ToArgs($"--wlon {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(ToRadians(val), cliParams.widgetLon);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.widgetLon);
            });
        }

        [Test]
        public void SetWDay()
        {
            var val = 10;
            var args = ToArgs($"--wday {val}");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(val, cliParams.widgetDay);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(0, cliParams.widgetDay);
            });
        }

        [Test]
        public void SetWSmartSpacing()
        {
            var args = ToArgs($"--wnaivespacing");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(true, cliParams.widgetNaiveSpacing);
            });

            args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.AreEqual(false, cliParams.widgetNaiveSpacing);
            });

        }

        [Test]
        public void ProcessParamsTest()
        {
            var args = ToArgs($"");
            Parse(args, cliParams =>
            {
                Assert.DoesNotThrow(() =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Assert.True(success);
                    Assert.True(projectionParams.SourceImage != null);
                });

            });

        }

    }
}
