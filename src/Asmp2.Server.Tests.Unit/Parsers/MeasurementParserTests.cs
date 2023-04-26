﻿using Asmp2.Server.Model;
using Asmp2.Server.Parsers;
using Asmp2.Shared.Model;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Asmp2.Server.Tests.Unit.Parsers
{
    public class MeasurementParserTests
    {

        private MeasurementParser _sut = new MeasurementParser();

        [Fact]
        public void Parse_WhenValidRawMeasurement_ThenParseResult()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "/KFM5KAIFA-METER",
                    "",
                    "1-3:0.2.8(42)",
                    "0-0:1.0.0(211116211905W)",
                    "0-0:96.1.1(4530303237303030303130313334353136)",
                    "1-0:1.8.1(008944.105*kWh)",
                    "1-0:1.8.2(010081.256*kWh)",
                    "1-0:2.8.1(000000.000*kWh)",
                    "1-0:2.8.2(000000.000*kWh)",
                    "0-0:96.14.0(0002)",
                    "1-0:1.7.0(00.449*kW)",
                    "1-0:2.7.0(00.000*kW)",
                    "0-0:96.7.21(00001)",
                    "0-0:96.7.9(00001)",
                    "1-0:99.97.0(2)(0-0:96.7.19)(170504154310S)(0000003453*s)(000101000001W)(2147483647*s)",
                    "1-0:32.32.0(00000)",
                    "1-0:32.36.0(00000)",
                    "0-0:96.13.1()",
                    "0-0:96.13.0()",
                    "1-0:31.7.0(002*A)",
                    "1-0:21.7.0(00.449*kW)",
                    "1-0:22.7.0(00.000*kW)",
                    "!3E4B"
                }
            };

            var result = _sut.Parse(input);

            result.Should().NotBeNull();
        }

        [Fact]
        public void Parse_WhenContainsCurrentUsage_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:1.7.0(00.449*kW)"
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerUsage.Current.Should().Be((decimal)0.449);
        }

        [Fact]
        public void Parse_WhenContainsCurrenSupply_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:2.7.0(00.449*kW)"
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerSupply.Current.Should().Be((decimal)0.449);
        }

        [Fact]
        public void Parse_WhenContainsTotalUsageLow_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:1.8.1(008944.105*kWh)"
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerUsage.TotalLow.Should().Be((decimal)8944.105);
        }

        [Fact]
        public void Parse_WhenContainsTotalUsageRegular_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:1.8.2(010081.256*kWh)",
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerUsage.TotalRegular.Should().Be((decimal)10081.256);
        }

        [Fact]
        public void Parse_WhenContainsTotalSupplyLow_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:2.8.1(000003.100*kWh)"
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerSupply.TotalLow.Should().Be((decimal)3.1);
        }

        [Fact]
        public void Parse_WhenContainsTotalSupplyRegular_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "1-0:2.8.2(000006.200*kWh)"
                }
            };

            var result = _sut.Parse(input);

            result.PowerUsage.Should().NotBeNull();
            result.PowerSupply.TotalRegular.Should().Be((decimal)6.2);
        }

        [Fact]
        public void Parse_WhenContainsTotalGasUsage_ThenParseCorrectly()
        {
            var input = new RawMeasurement
            {
                Lines = new List<string>
                {
                    "0-1:24.2.1(00500.34*m3)"
                }
            };

            var result = _sut.Parse(input);

            result.GasUsage.Should().NotBeNull();
            result.GasUsage.Total.Should().Be((decimal)500.34);
        }
    }
}
