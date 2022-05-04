using System;
using Xunit;

namespace Cron.Tests
{
    public class CronTest
    {
        [Theory]
        [InlineData("* * * * *", "2022/05/01 00:00:00")]
        [InlineData("0 0 1 5 0", "2022/05/01 00:00:00")]
        [InlineData("1,3,5 * * * *", "2022/05/01 00:01:00")]
        [InlineData("1,3,5 * * * *", "2022/05/01 00:03:00")]
        [InlineData("1,3,5 * * * *", "2022/05/01 00:05:00")]

        [InlineData("* 5-10 * * *", "2022/05/01 05:00:00")]
        [InlineData("* 5-10 * * *", "2022/05/01 10:59:00")]
        [InlineData("* 5-10,15 * * *", "2022/05/01 15:00:00")]
        [InlineData("*/30 * * * *", "2022/05/01 00:00:00")]
        [InlineData("*/30 * * * *", "2022/05/01 00:30:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 01:00:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 03:00:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 05:00:00")]

        [InlineData("* * * jan *", "2022/01/01 00:00:00")]
        [InlineData("* * * feb *", "2022/02/01 00:00:00")]
        [InlineData("* * * mar *", "2022/03/01 00:00:00")]
        [InlineData("* * * apr *", "2022/04/01 00:00:00")]
        [InlineData("* * * may *", "2022/05/01 00:00:00")]
        [InlineData("* * * june *", "2022/06/01 00:00:00")]
        [InlineData("* * * july *", "2022/07/01 00:00:00")]
        [InlineData("* * * aug *", "2022/08/01 00:00:00")]
        [InlineData("* * * sept *", "2022/09/01 00:00:00")]
        [InlineData("* * * oct *", "2022/10/01 00:00:00")]
        [InlineData("* * * nov *", "2022/11/01 00:00:00")]
        [InlineData("* * * dec *", "2022/12/01 00:00:00")]

        [InlineData("* * * * sun", "2022/05/01 00:00:00")]
        [InlineData("* * * * Mon", "2022/05/02 00:00:00")]
        [InlineData("* * * * tue", "2022/05/03 00:00:00")]
        [InlineData("* * * * wed", "2022/05/04 00:00:00")]
        [InlineData("* * * * thu", "2022/05/05 00:00:00")]
        [InlineData("* * * * fri", "2022/05/06 00:00:00")]
        [InlineData("* * * * sat", "2022/05/07 00:00:00")]
        public void Verify_TrueTest(string expression, string datetime)
        {
            var cron = new Cron(expression);
            var dt = DateTime.Parse(datetime);

            var result = cron.Verify(dt);

            Assert.True(result);
        }

        [Theory]
        [InlineData("1 * * * *", "2022/05/01 00:00:00")]
        [InlineData("* 1 * * *", "2022/05/01 00:00:00")]
        [InlineData("* * 2 * *", "2022/05/01 00:00:00")]
        [InlineData("* * * 4 *", "2022/05/01 00:00:00")]
        [InlineData("* * * 6 *", "2022/05/01 00:00:00")]
        [InlineData("* * * * 1", "2022/05/01 00:00:00")]

        [InlineData("1,3,5 * * * *", "2022/05/01 00:00:00")]
        [InlineData("1,3,5 * * * *", "2022/05/01 00:02:00")]
        [InlineData("1,3,5 * * * *", "2022/05/01 00:04:00")]

        [InlineData("* 5-10 * * *", "2022/05/01 04:59:00")]
        [InlineData("* 5-10 * * *", "2022/05/01 11:00:00")]
        [InlineData("*/30 * * * *", "2022/05/01 00:01:00")]
        [InlineData("*/30 * * * *", "2022/05/01 00:31:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 02:00:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 04:00:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 06:00:00")]
        [InlineData("* 1-5/2 * * *", "2022/05/01 07:00:00")]
        public void Verify_FalseTest(string expression, string datetime)
        {
            var cron = new Cron(expression);
            var dt = DateTime.Parse(datetime);

            var result = cron.Verify(dt);

            Assert.False(result);
        }
    }
}
