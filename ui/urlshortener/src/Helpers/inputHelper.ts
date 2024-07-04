const inputHelper = (e : React.ChangeEvent<HTMLInputElement>, data: any) => {
    e.preventDefault();

    const tempData: any = {...data};
    
    tempData[e.target.name] = e.target.value;

    return tempData;
}

export default inputHelper;