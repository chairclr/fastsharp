RWTexture2D<unorm float4> TestTexture : register(u0);
RWTexture1D<unorm float4> Test1DTexture : register(u1);


Texture2D<unorm float4> TestSRV : register (t0);

struct TestStruct
{
    int X;
    int Length;
    int2 __padding;
};

RWStructuredBuffer<TestStruct> TestRWBuffer : register(u2);

Buffer<float4> TestResourceBuffer : register(t1);

[numthreads(16, 16, 1)]
void CSMain(uint3 id : SV_DispatchThreadID)
{
    uint width;
    uint height;
    TestTexture.GetDimensions(width, height);

    int3 coords = int3(id.x % 2, id.y % 2, 0);
    
    float4 color = TestSRV.Load(coords);
    
    TestTexture[id.xy] = color;
    Test1DTexture[id.x + id.y] = TestResourceBuffer[id.x % 2];
    
    TestStruct ts = TestRWBuffer[id.x];
    
    for (int i = 0; i < ts.Length; i++)
    {
        ts.X += ts.Length;
    }
    
    TestRWBuffer[id.x] = ts;
}