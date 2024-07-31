function moran_scatter=moran_test_y(x,w) 
zx=(x-mean(x))/std(x);
wzx=w*zx;
scatter(zx,wzx,'filled');
axis([-3,4,-0.6,0.8])
hold on

n=xlim;
m=ylim;
moran_I=regress(wzx,zx);
zx1=-3:0.01:4;

moran_scatter=plot(zx1,moran_I*zx1,'-r','linewidth',2);
title(['Moran’s I = ',num2str(moran_I)]);
hold on

line([n(1),n(2)],[0,0],'linestyle','--','color','k');
line([0,0],[m(1),m(2)],'linestyle','--','color','k');
end
