Texture2D<unorm float4> ImmutableTexture : register(t0);
RWTexture2D<unorm float4> OutputTexture : register(u0);

[numthreads(2, 2, 1)]
void CSMain(uint3 id : SV_DispatchThreadID)
{
    OutputTexture[id.xy] = ImmutableTexture[id.xy];
}