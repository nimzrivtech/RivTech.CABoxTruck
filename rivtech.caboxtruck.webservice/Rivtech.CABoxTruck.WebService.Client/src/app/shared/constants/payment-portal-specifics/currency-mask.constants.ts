const _addDecimal = (places: number) => {
    return { precision: places };
}

const _allowNegative = (allow: boolean) => {
    let option = {
        precision: 2,
        allowNegative: false
    }

    if(allow){
        option.allowNegative = true
    }

    return option;
}

export const CurrencyMaskConstants = {
    NoPrefix: { prefix: "" },
    AddDecimal: _addDecimal,
    TwoDecimal: _addDecimal(2),
    Integer: {
        prefix: "",
        thousands: ""
    },
    TwoDecimalAndAllowNegative: _allowNegative(true),
    TwoDecimalAndNoNegative: _allowNegative(false),
    Percentage: {
      prefix: "",
      suffix: "%"
    }
};