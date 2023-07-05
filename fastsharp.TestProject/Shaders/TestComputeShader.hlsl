RWTexture2D<unorm float4> TestTexture : register(u0);
Texture2D<unorm float4> TestSRV : register (t0);

[numthreads(16, 16, 1)]
void CSMain(uint3 id : SV_DispatchThreadID)
{
    uint width;
    uint height;
    TestTexture.GetDimensions(width, height);

    int3 coords = int3(id.x % 2, id.y % 2, 0);
    
    float4 color = TestSRV.Load(coords);
    
    TestTexture[id.xy] = color;
}