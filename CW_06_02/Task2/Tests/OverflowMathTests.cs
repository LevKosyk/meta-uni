using Xunit;

namespace Task2.Overflow.Tests;

public class OverflowMathTests
{
    [Fact]
    public void AddChecked_MaxValuePlusOne_ThrowsOverflowException()
    {
        Assert.Throws<OverflowException>(() => OverflowMath.AddChecked(int.MaxValue, 1));
    }

    [Fact]
    public void AddChecked_MinValueMinusOne_ThrowsOverflowException()
    {
        Assert.Throws<OverflowException>(() => OverflowMath.AddChecked(int.MinValue, -1));
    }

    [Fact]
    public void AddChecked_ExactlyAtBoundary_DoesNotThrow()
    {
        Assert.Equal(int.MaxValue, OverflowMath.AddChecked(int.MaxValue - 1, 1));
        Assert.Equal(int.MinValue, OverflowMath.AddChecked(int.MinValue + 1, -1));
    }

    [Fact]
    public void AddWrapped_MaxValuePlusOne_WrapsToMinValue()
    {
        Assert.Equal(int.MinValue, OverflowMath.AddWrapped(int.MaxValue, 1));
    }

    [Fact]
    public void AddWrapped_MinValueMinusOne_WrapsToMaxValue()
    {
        Assert.Equal(int.MaxValue, OverflowMath.AddWrapped(int.MinValue, -1));
    }

    [Fact]
    public void AddDefault_MaxValuePlusOne_DependsOnBuildConfiguration()
    {
#if DEBUG
        Assert.Throws<OverflowException>(() => OverflowMath.AddDefault(int.MaxValue, 1));
#else
        Assert.Equal(int.MinValue, OverflowMath.AddDefault(int.MaxValue, 1));
#endif
    }

    [Fact]
    public void SumAll_SafeMode_ThrowsOnOverflow()
    {
        int[] values = [int.MaxValue, 1];

        Assert.Throws<OverflowException>(() => OverflowMath.SumAll(values, safe: true));
    }

    [Fact]
    public void SumAll_FastMode_Wraps()
    {
        int[] values = [int.MaxValue, int.MaxValue];

        Assert.Equal(-2, OverflowMath.SumAll(values, safe: false));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SumAll_NoOverflow_SameResultInBothModes(bool safe)
    {
        int[] values = [int.MaxValue, int.MinValue, 10];

        Assert.Equal(9, OverflowMath.SumAll(values, safe));
    }

    [Fact]
    public void SumAll_Null_ThrowsArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => OverflowMath.SumAll(null!, safe: true));

        Assert.Equal("values", ex.ParamName);
    }
}
