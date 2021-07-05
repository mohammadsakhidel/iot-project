import React from 'react'

export default function MainLayout(props: any) {

    const {
        children
    } = props;

    return (
        <div style={{ border: 'solid 1px red' }}>
            {children}
        </div>
    )
}
