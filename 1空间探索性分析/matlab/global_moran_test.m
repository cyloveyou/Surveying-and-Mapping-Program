% x0：一个矩阵，列代表每一个字段
% w： 空间权重矩阵
function global_moran_test(x0, w)
    row = size(x0, 2); % 输入的x是一个矩阵，每一列是一个字段，按照列来遍历计算莫兰指数
    moran.mean = zeros(row, 1);% 存储每一列的均值
    moran.num = zeros(row, 1);  % 
    moran.stdev = zeros(row, 1); % 标准差
    moran.index = zeros(row, 1);
    moran.z = zeros(row, 1);
    moran.p = zeros(row, 1);

    moran.globalresult = zeros(row, 3);

    for r = 1:1:row
        x = x0(:, r);
        moran.mean(r) = mean(x); %计算x均值
        moran.num(r) = size(x, 1); % 计算每一个字段的元素总数
        moran.stdev(r) = std(x); % 计算标准差

        z_x = (x - moran.mean(r)) / moran.stdev(r); %z值，为 x的每一个数字，减去均值，除标准差
        sum_wij = 0;
        s = 0;
        s1 = 0;
        s2 = 0;
        m2 = 0;
        m4 = 0;
        % 计算需要权重矩阵的那一部分
        for a = 1:1:moran.num(r)
            w_i = 0;
            w_j = 0;
            m2 = m2 + (x(a, 1) - moran.mean(r))^2;
            m4 = m4 + (x(a, 1) - moran.mean(r))^4;
            for b = 1:1:moran.num(r)
                sum_wij = sum_wij + (w(a, b) * z_x(a, 1) * z_x(b, 1));
                s = s + w(a, b);
                s1 = s1 + (w(a, b) + w(b, a))^2;
                w_i = w_i + w(a, b);
                w_j = w_j + w(b, a);
            end
            s2 = s2 + (w_i + w_j)^2;
        end
        m2 = m2 / moran.num(r);
        m4 = m4 / moran.num(r);
        b2 = m4 / (m2^2);
        sum_i2 = 0;
        for a = 1:1:moran.num(r)
            sum_i2 = sum_i2 + (z_x(a, 1) * z_x(a, 1));
        end
        moran.index(r) = (moran.num(r) * sum_wij) / (s * sum_i2);

        n = moran.num(r);
        temp_1 = n * (n^2 - 3 * n + 3) * s1 - (n * s2) + (3 * s^2);
        temp_2 = b2 * ((n^2 - n) * s1 - (2 * n * s2) + (6 * s^2));
        den = (n - 1) * (n - 2) * (n - 3) * (s^2);
        sd = (temp_1 - temp_2) / den - (1 / (n - 1))^2;
        sd = sqrt(sd);

        e = -1 / (n - 1);

        moran.z(r) = (moran.index(r) - e) / sd;

        moran.p(r) = 1 - normcdf(moran.z(r));
    end

    moran.globalresult(:, 1) = moran.index;
    moran.globalresult(:, 2) = moran.z;
    moran.globalresult(:, 3) = moran.p;

    fprintf('%6s %12s %18s %24s\r\n', 't', 'Moran', 'z', 'p');
    for i = 1:1:row
        fprintf('%6.3f %12.3f %18.3f %24.3\n', i, moran.index(i), moran.z(i), moran.p(i));
    end

end
