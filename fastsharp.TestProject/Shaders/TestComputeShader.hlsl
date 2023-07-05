RWTexture2D<unorm float4> TestTexture : register(u0);

[numthreads(16, 16, 1)]
void CSMain(uint3 id : SV_DispatchThreadID)
{
    uint width;
    uint height;
    TestTexture.GetDimensions(width, height);
    
    float2 uv = float2(id.x, id.y) / float2(width, height);
    
    TestTexture[id.xy] = float4(uv.xyx, 1.0);
}